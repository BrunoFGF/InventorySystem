import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';
import { LoginRequest } from '../../interfaces/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginRequest: LoginRequest = { email: '', password: '' };
  hidePassword = true;
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  onSubmit(): void {
    if (!this.loginRequest.email || !this.loginRequest.password) {
      this.snackBar.open('Complete todos los campos.', 'Cerrar', { duration: 3000 });
      return;
    }

    this.loading = true;
    this.authService.login(this.loginRequest).subscribe({
      next: (response) => {
        if (response.success) {
          this.router.navigate(['/products']);
        }
      },
      error: (err) => {
        this.loading = false;
        const message = err.error?.message || 'Error al iniciar sesión.';
        this.snackBar.open(message, 'Cerrar', { duration: 3000 });
      }
    });
  }
}