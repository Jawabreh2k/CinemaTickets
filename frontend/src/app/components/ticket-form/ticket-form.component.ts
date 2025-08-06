import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Ticket, CreateTicket, UpdateTicket } from '../../models/ticket.model';
import { Showtime } from '../../models/showtime.model';
import { TicketService } from '../../services/ticket.service';
import { ShowtimeService } from '../../services/showtime.service';

@Component({
  selector: 'app-ticket-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
  ],
  templateUrl: './ticket-form.component.html',
  styleUrl: './ticket-form.component.scss',
})
export class TicketFormComponent implements OnInit {
  ticketForm: FormGroup;
  showtimes: Showtime[] = [];
  isEditMode = false;
  ticketId: number | null = null;
  availableSeats: number[] = [];

  constructor(
    private fb: FormBuilder,
    private ticketService: TicketService,
    private showtimeService: ShowtimeService,
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.ticketForm = this.fb.group({
      showtimeId: ['', Validators.required],
      customerName: ['', [Validators.required, Validators.minLength(2)]],
      customerEmail: ['', [Validators.required, Validators.email]],
      phoneNumber: [
        '',
        [Validators.required, Validators.pattern(/^[\+]?[1-9][\d]{0,15}$/)],
      ],
      seatNumber: ['', [Validators.required, Validators.min(1)]],
    });
  }

  ngOnInit(): void {
    this.loadShowtimes();

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.ticketId = +id;
      this.loadTicket(this.ticketId);
    }

    // Listen for showtime changes to update available seats
    this.ticketForm.get('showtimeId')?.valueChanges.subscribe((showtimeId) => {
      if (showtimeId) {
        this.updateAvailableSeats(showtimeId);
      }
    });
  }

  loadShowtimes(): void {
    this.showtimeService.getAvailableShowtimes().subscribe({
      next: (showtimes) => {
        this.showtimes = showtimes;
      },
      error: (error) => {
        this.snackBar.open('Error loading showtimes', 'Close', {
          duration: 3000,
        });
        console.error('Error loading showtimes:', error);
      },
    });
  }

  loadTicket(id: number): void {
    this.ticketService.getTicket(id).subscribe({
      next: (ticket) => {
        this.ticketForm.patchValue({
          showtimeId: ticket.showtimeId,
          customerName: ticket.customerName,
          customerEmail: ticket.customerEmail,
          phoneNumber: ticket.phoneNumber,
          seatNumber: ticket.seatNumber,
        });
        this.updateAvailableSeats(ticket.showtimeId);
      },
      error: (error) => {
        this.snackBar.open('Error loading ticket', 'Close', { duration: 3000 });
        console.error('Error loading ticket:', error);
      },
    });
  }

  updateAvailableSeats(showtimeId: number): void {
    const showtime = this.showtimes.find((s) => s.id === showtimeId);
    if (showtime) {
      this.availableSeats = Array.from(
        { length: showtime.availableSeats },
        (_, i) => i + 1
      );
    }
  }

  onSubmit(): void {
    if (this.ticketForm.valid) {
      const formValue = this.ticketForm.value;

      if (this.isEditMode && this.ticketId) {
        const updateTicket: UpdateTicket = {
          customerName: formValue.customerName,
          customerEmail: formValue.customerEmail,
          phoneNumber: formValue.phoneNumber,
          seatNumber: formValue.seatNumber,
          status: 'Active',
        };

        this.ticketService.updateTicket(this.ticketId, updateTicket).subscribe({
          next: () => {
            this.snackBar.open('Ticket updated successfully', 'Close', {
              duration: 3000,
            });
            this.router.navigate(['/tickets']);
          },
          error: (error) => {
            this.snackBar.open(
              error.error || 'Error updating ticket',
              'Close',
              { duration: 3000 }
            );
            console.error('Error updating ticket:', error);
          },
        });
      } else {
        const createTicket: CreateTicket = {
          showtimeId: formValue.showtimeId,
          customerName: formValue.customerName,
          customerEmail: formValue.customerEmail,
          phoneNumber: formValue.phoneNumber,
          seatNumber: formValue.seatNumber,
        };

        this.ticketService.createTicket(createTicket).subscribe({
          next: () => {
            this.snackBar.open('Ticket created successfully', 'Close', {
              duration: 3000,
            });
            this.router.navigate(['/tickets']);
          },
          error: (error) => {
            this.snackBar.open(
              error.error || 'Error creating ticket',
              'Close',
              { duration: 3000 }
            );
            console.error('Error creating ticket:', error);
          },
        });
      }
    } else {
      this.markFormGroupTouched();
    }
  }

  onCancel(): void {
    this.router.navigate(['/tickets']);
  }

  private markFormGroupTouched(): void {
    Object.keys(this.ticketForm.controls).forEach((key) => {
      const control = this.ticketForm.get(key);
      control?.markAsTouched();
    });
  }

  getErrorMessage(controlName: string): string {
    const control = this.ticketForm.get(controlName);
    if (control?.hasError('required')) {
      return `${controlName} is required`;
    }
    if (control?.hasError('email')) {
      return 'Please enter a valid email address';
    }
    if (control?.hasError('minlength')) {
      return `${controlName} must be at least ${control.errors?.['minlength'].requiredLength} characters`;
    }
    if (control?.hasError('pattern')) {
      return 'Please enter a valid phone number';
    }
    if (control?.hasError('min')) {
      return `${controlName} must be at least ${control.errors?.['min'].min}`;
    }
    return '';
  }
}
