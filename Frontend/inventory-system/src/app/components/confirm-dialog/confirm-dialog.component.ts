import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface ConfirmDialogData {
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
}

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatIconModule],
  template: `
    <div class="dialog-header">
      <mat-icon class="warn-icon">warning_amber</mat-icon>
      <h2 mat-dialog-title>{{ data.title }}</h2>
    </div>

    <mat-dialog-content>
      <p>{{ data.message }}</p>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-stroked-button (click)="ref.close(false)">
        {{ data.cancelText ?? 'Cancelar' }}
      </button>
      <button mat-raised-button color="warn" (click)="ref.close(true)">
        {{ data.confirmText ?? 'Eliminar' }}
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    .dialog-header {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      padding: 1.5rem 1.5rem 0;
    }
    .warn-icon {
      color: #e65100;
      font-size: 1.75rem;
      width: 1.75rem;
      height: 1.75rem;
    }
    h2 {
      margin: 0;
      font-size: 1.1rem;
      padding: 0;
    }
    mat-dialog-content p {
      color: #555;
      margin: 0;
    }
    mat-dialog-actions {
      padding: 0.75rem 1.5rem 1.25rem;
      gap: 0.5rem;
    }
  `]
})
export class ConfirmDialogComponent {
  data = inject<ConfirmDialogData>(MAT_DIALOG_DATA);
  ref = inject(MatDialogRef<ConfirmDialogComponent>);
}
