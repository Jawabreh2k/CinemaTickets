using System.ComponentModel.DataAnnotations;

namespace cinema_tickets.Models;

public class Movie
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public int Duration { get; set; } // Duration in minutes
    
    [StringLength(50)]
    public string? Genre { get; set; }
    
    [StringLength(10)]
    public string? Rating { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    // Navigation property
    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
} 