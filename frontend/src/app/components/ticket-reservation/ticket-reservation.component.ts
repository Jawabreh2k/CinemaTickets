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
import { Router } from '@angular/router';
import { Showtime } from '../../models/showtime.model';
import { CreateTicket } from '../../models/ticket.model';
import { TicketService } from '../../services/ticket.service';
import { ShowtimeService } from '../../services/showtime.service';

@Component({
  selector: 'app-ticket-reservation',
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
  templateUrl: './ticket-reservation.component.html',
  styleUrl: './ticket-reservation.component.scss',
})
export class TicketReservationComponent implements OnInit {
  reservationForm: FormGroup;
  showtimes: Showtime[] = [];
  selectedShowtime: Showtime | null = null;
  availableSeats: number[] = [];
  selectedSeats: number[] = [];
  totalPrice: number = 0;

  constructor(
    private fb: FormBuilder,
    private ticketService: TicketService,
    private showtimeService: ShowtimeService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {
    this.reservationForm = this.fb.group({
      showtimeId: ['', Validators.required],
      customerName: ['', [Validators.required, Validators.minLength(2)]],
      customerEmail: ['', [Validators.required, Validators.email]],
      phoneNumber: [
        '',
        [Validators.required, Validators.pattern(/^[\+]?[1-9][\d]{0,15}$/)],
      ],
      seatNumbers: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadShowtimes();

    // Listen for showtime changes
    this.reservationForm
      .get('showtimeId')
      ?.valueChanges.subscribe((showtimeId) => {
        if (showtimeId) {
          this.updateSelectedShowtime(showtimeId);
        }
      });

    // Listen for seat selection changes
    this.reservationForm.get('seatNumbers')?.valueChanges.subscribe((seats) => {
      this.selectedSeats = seats || [];
      this.calculateTotalPrice();
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

  updateSelectedShowtime(showtimeId: number): void {
    this.selectedShowtime =
      this.showtimes.find((s) => s.id === showtimeId) || null;
    if (this.selectedShowtime) {
      this.availableSeats = Array.from(
        { length: this.selectedShowtime.availableSeats },
        (_, i) => i + 1
      );
      this.calculateTotalPrice();
    }
  }

  calculateTotalPrice(): void {
    if (this.selectedShowtime && this.selectedSeats.length > 0) {
      this.totalPrice = this.selectedShowtime.price * this.selectedSeats.length;
    } else {
      this.totalPrice = 0;
    }
  }

  onSeatSelectionChange(event: any): void {
    const selectedSeats = event.value;
    this.selectedSeats = selectedSeats;
    this.calculateTotalPrice();
  }

  onSubmit(): void {
    if (this.reservationForm.valid && this.selectedSeats.length > 0) {
      const formValue = this.reservationForm.value;
      const reservations: CreateTicket[] = [];

      // Create a reservation for each selected seat
      for (const seatNumber of this.selectedSeats) {
        const reservation: CreateTicket = {
          showtimeId: formValue.showtimeId,
          customerName: formValue.customerName,
          customerEmail: formValue.customerEmail,
          phoneNumber: formValue.phoneNumber,
          seatNumber: seatNumber,
        };
        reservations.push(reservation);
      }

      // Process reservations sequentially
      this.processReservations(reservations, 0);
    } else {
      this.markFormGroupTouched();
    }
  }

  private processReservations(
    reservations: CreateTicket[],
    index: number
  ): void {
    if (index >= reservations.length) {
      this.snackBar.open(
        `${reservations.length} ticket(s) reserved successfully!`,
        'Close',
        { duration: 3000 }
      );
      this.router.navigate(['/tickets']);
      return;
    }

    this.ticketService.createTicket(reservations[index]).subscribe({
      next: () => {
        this.processReservations(reservations, index + 1);
      },
      error: (error) => {
        this.snackBar.open(
          `Error reserving seat ${reservations[index].seatNumber}: ${error.error}`,
          'Close',
          { duration: 3000 }
        );
        console.error('Error creating ticket:', error);
      },
    });
  }

  onCancel(): void {
    this.router.navigate(['/tickets']);
  }

  private markFormGroupTouched(): void {
    Object.keys(this.reservationForm.controls).forEach((key) => {
      const control = this.reservationForm.get(key);
      control?.markAsTouched();
    });
  }

  getErrorMessage(controlName: string): string {
    const control = this.reservationForm.get(controlName);
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
    return '';
  }
}
