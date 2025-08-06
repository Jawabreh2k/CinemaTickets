namespace cinema_tickets.DTOs;

public class ShowtimeDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Hall { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
}

public class CreateShowtimeDto
{
    public int MovieId { get; set; }
    public DateTime StartTime { get; set; }
    public string Hall { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalSeats { get; set; }
} 