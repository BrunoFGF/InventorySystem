import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const snackBar = inject(MatSnackBar);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        authService.logout();
        router.navigate(['/login']);
        snackBar.open('Sesión expirada. Inicie sesión nuevamente.', 'Cerrar', { duration: 4000 });
      } else if (error.status === 0) {
        console.error('Network error:', error);
        snackBar.open('Error de conexión. Verifique su conexión a internet.', 'Cerrar', { duration: 4000 });
      } else {
        console.error(`HTTP ${error.status} - ${req.method} ${req.url}:`, error);
      }

      return throwError(() => error);
    })
  );
};
