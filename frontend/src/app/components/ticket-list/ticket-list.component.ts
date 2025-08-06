import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router } from '@angular/router';
import { Ticket } from '../../models/ticket.model';
import { TicketService } from '../../services/ticket.service';
import { Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatCardModule,
    MatTooltipModule,
  ],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.scss',
})
export class TicketListComponent implements OnInit {
  tickets: Ticket[] = [];
  displayedColumns: string[] = [
    'id',
    'movieTitle',
    'customerName',
    'customerEmail',
    'seatNumber',
    'price',
    'status',
    'actions',
  ];

  constructor(
    private ticketService: TicketService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.ticketService.getTickets().subscribe({
      next: (tickets) => {
        this.tickets = tickets;
      },
      error: (error) => {
        this.snackBar.open('Error loading tickets', 'Close', {
          duration: 3000,
        });
        console.error('Error loading tickets:', error);
      },
    });
  }

  editTicket(ticket: Ticket): void {
    this.router.navigate(['/tickets/edit', ticket.id]);
  }

  deleteTicket(ticket: Ticket): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px',
      data: {
        message: `Are you sure you want to delete ticket for ${ticket.customerName}?`,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.ticketService.deleteTicket(ticket.id).subscribe({
          next: () => {
            this.snackBar.open('Ticket deleted successfully', 'Close', {
              duration: 3000,
            });
            this.loadTickets();
          },
          error: (error) => {
            this.snackBar.open('Error deleting ticket', 'Close', {
              duration: 3000,
            });
            console.error('Error deleting ticket:', error);
          },
        });
      }
    });
  }

  addTicket(): void {
    this.router.navigate(['/tickets/add']);
  }

  reserveTicket(): void {
    this.router.navigate(['/tickets/reserve']);
  }

  getStatusIcon(status: string): string {
    switch (status.toLowerCase()) {
      case 'active':
        return 'check_circle';
      case 'cancelled':
        return 'cancel';
      case 'used':
        return 'event_available';
      default:
        return 'help';
    }
  }
}

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Confirm Delete</h2>
    <mat-dialog-content>{{ data.message }}</mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-button [mat-dialog-close]="true" color="warn">Delete</button>
    </mat-dialog-actions>
  `,
})
export class ConfirmDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: { message: string }) {}
}
