using System.ComponentModel.DataAnnotations;

namespace cinema_tickets.Models;

public class Ticket
{
    public int Id { get; set; }
    
    [Required]
    public int ShowtimeId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string CustomerEmail { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    public int SeatNumber { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    
    [StringLength(20)]
    public string Status { get; set; } = "Active"; // Active, Cancelled, Used
    
    // Navigation property
    public Showtime Showtime { get; set; } = null!;
} 