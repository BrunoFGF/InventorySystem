import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { ProductService } from '../../services/product.service';
import { AuthService } from '../../services/auth.service';
import { Product } from '../../interfaces/product';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatCardModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTooltipModule,
    MatChipsModule
  ],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  loading = true;

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
          this.products = response.data;
        }
        this.loading = false;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error:', err);
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