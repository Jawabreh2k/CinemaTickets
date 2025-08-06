import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Showtime } from '../models/showtime.model';

@Injectable({
  providedIn: 'root',
})
export class ShowtimeService {
  private apiUrl = 'https://localhost:7063/api/showtimes';

  constructor(private http: HttpClient) {}

  getShowtimes(): Observable<Showtime[]> {
    return this.http.get<Showtime[]>(this.apiUrl);
  }

  getAvailableShowtimes(): Observable<Showtime[]> {
    return this.http.get<Showtime[]>(`${this.apiUrl}/available`);
  }

  getShowtime(id: number): Observable<Showtime> {
    return this.http.get<Showtime>(`${this.apiUrl}/${id}`);
  }
}
