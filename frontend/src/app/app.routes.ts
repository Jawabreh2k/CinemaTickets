import { Routes } from '@angular/router';
import { TicketListComponent } from './components/ticket-list/ticket-list.component';
import { TicketFormComponent } from './components/ticket-form/ticket-form.component';
import { TicketReservationComponent } from './components/ticket-reservation/ticket-reservation.component';

export const routes: Routes = [
  { path: '', redirectTo: '/tickets', pathMatch: 'full' },
  { path: 'tickets', component: TicketListComponent },
  { path: 'tickets/add', component: TicketFormComponent },
  { path: 'tickets/edit/:id', component: TicketFormComponent },
  { path: 'tickets/reserve', component: TicketReservationComponent },
  { path: '**', redirectTo: '/tickets' },
];
