export interface Showtime {
  id: number;
  movieId: number;
  movieTitle: string;
  startTime: Date;
  endTime: Date;
  hall: string;
  price: number;
  totalSeats: number;
  availableSeats: number;
}
