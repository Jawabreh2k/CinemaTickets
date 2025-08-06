namespace cinema_tickets.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public int ShowtimeId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public string Status { get; set; } = string.Empty;
    
    // Additional properties for display
    public string MovieTitle { get; set; } = string.Empty;
    public DateTime ShowtimeStartTime { get; set; }
    public string Hall { get; set; } = string.Empty;
}

public class CreateTicketDto
{
    public int ShowtimeId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
}

public class UpdateTicketDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public string Status { get; set; } = string.Empty;
} 

