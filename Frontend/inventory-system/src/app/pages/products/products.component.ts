import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ProductService } from '../../services/product.service';
import { AuthService } from '../../services/auth.service';
import { Product } from '../../interfaces/product';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatCardModule,
    MatSnackBarModule,
    MatTooltipModule,
  ],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit {
  displayedColumns = ['name', 'suppliers', 'stock', 'createdAt', 'actions'];
  dataSource = new MatTableDataSource<Product>([]);
  loading = true;

  @ViewChild(MatPaginator) set paginator(paginator: MatPaginator) {
    if (paginator) this.dataSource.paginator = paginator;
  }

  constructor(
    private productService: ProductService,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.dataSource.data = response.data;
        }
        this.loading = false;
        this.cdr.markForCheck();
      },
      error: () => {
        this.snackBar.open('Error al cargar los productos.', 'Cerrar', { duration: 3000 });
        this.loading = false;
        this.cdr.markForCheck();
      }
    });
  }

  newProduct(): void {
    this.router.navigate(['/products/new']);
  }

  editProduct(id: number): void {
    this.router.navigate(['/products/edit', id]);
  }

  deleteProduct(product: Product): void {
    if (!confirm(`¿Está seguro de eliminar "${product.name}"?`)) return;

    this.productService.delete(product.id).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open('Producto eliminado exitosamente.', 'Cerrar', { duration: 3000 });
          this.loadProducts();
        }
        this.cdr.markForCheck();
      },
      error: () => {
        this.snackBar.open('Error al eliminar el producto.', 'Cerrar', { duration: 3000 });
        this.cdr.markForCheck();
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  getTotalStock(product: Product): number {
    return product.suppliers.reduce((total, s) => total + s.stock, 0);
  }
}
