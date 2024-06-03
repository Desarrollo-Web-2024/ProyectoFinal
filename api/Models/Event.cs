using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class Event {
    public required int Id { get; set; }
    [MaxLength(60)] public required string Name { get; set; }
    [MaxLength(500)] public required string Description { get; set; }
    public required DateTime StartDate { get; set; }
    public required int Duration { get; set; }
    public required bool Solved { get; set; }
    public required User Client { get; set; } 
}