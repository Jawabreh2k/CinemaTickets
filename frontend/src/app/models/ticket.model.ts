export interface Ticket {
  id: number;
  showtimeId: number;
  customerName: string;
  customerEmail: string;
  phoneNumber: string;
  seatNumber: number;
  price: number;
  purchaseDate: Date;
  status: string;
  movieTitle: string;
  showtimeStartTime: Date;
  hall: string;
}

export interface CreateTicket {
  showtimeId: number;
  customerName: string;
  customerEmail: string;
  phoneNumber: string;
  seatNumber: number;
}

export interface UpdateTicket {
  customerName: string;
  customerEmail: string;
  phoneNumber: string;
  seatNumber: number;
  status: string;
}
