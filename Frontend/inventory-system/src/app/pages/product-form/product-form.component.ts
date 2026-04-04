import { ChangeDetectorRef, Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProductService } from '../../services/product.service';
import { SupplierService } from '../../services/supplier.service';
import { NotificationService } from '../../services/notification.service';
import { CreateProduct, CreateProductSupplier } from '../../interfaces/product';
import { Supplier } from '../../interfaces/supplier';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatSnackBarModule,
    MatToolbarModule,
    MatTooltipModule
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent implements OnInit {
  isEdit = false;
  productId: number | null = null;
  loading = signal(false);

  product: CreateProduct = {
    name: '',
    description: '',
    suppliers: []
  };

  suppliers: Supplier[] = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private supplierService: SupplierService,
    private notification: NotificationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadSuppliers();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.productId = +id;
      this.loadProduct(this.productId);
    }
  }

  loadSuppliers(): void {
    this.supplierService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.suppliers = response.data;
          this.cdr.markForCheck();
        }
      }
    });
  }

  loadProduct(id: number): void {
    this.productService.getById(id).subscribe({
      next: (response) => {
        if (response.success) {
          this.product = {
            name: response.data.name,
            description: response.data.description,
            suppliers: response.data.suppliers.map(s => ({
              supplierId: s.supplierId,
              price: s.price,
              stock: s.stock,
              batchNumber: s.batchNumber
            }))
          };
          this.cdr.markForCheck();
        }
      },
      error: () => {
        this.notification.error('Error al cargar el producto.');
        this.router.navigate(['/products']);
      }
    });
  }

  addSupplier(): void {
    this.product.suppliers.push({
      supplierId: 0,
      price: 0,
      stock: 0,
      batchNumber: ''
    });
  }

  removeSupplier(index: number): void {
    this.product.suppliers.splice(index, 1);
  }

  onSubmit(): void {
    if (!this.product.name) {
      this.notification.warning('El nombre es obligatorio.');
      return;
    }

    if (this.product.suppliers.length === 0) {
      this.notification.warning('Agregue al menos un proveedor.');
      return;
    }

    this.loading.set(true);

    const request = this.isEdit
      ? this.productService.update(this.productId!, this.product)
      : this.productService.create(this.product);

    request.subscribe({
      next: (response) => {
        if (response.success) {
          this.notification.success(this.isEdit ? 'Producto actualizado.' : 'Producto creado.');
          this.router.navigate(['/products']);
        } else {
          this.loading.set(false);
        }
      },
      error: (err) => {
        this.loading.set(false);
        const message = err.error?.message || 'Error al guardar el producto.';
        this.notification.error(message);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/products']);
  }
}