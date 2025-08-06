using System.ComponentModel.DataAnnotations;

namespace cinema_tickets.Models;

public class Showtime
{
    public int Id { get; set; }
    
    [Required]
    public int MovieId { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public DateTime EndTime { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Hall { get; set; } = string.Empty;
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public int TotalSeats { get; set; }
    
    public int AvailableSeats { get; set; }
    
    // Navigation properties
    public Movie Movie { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
} 