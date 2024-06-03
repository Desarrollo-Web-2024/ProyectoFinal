using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class User {
    public required int Id { get; set; }
    [MaxLength(60)]
    public required string Username { get; set; }
    [MaxLength(60)]
    public required string Name { get; set; }
    [MaxLength(60)]
    public required string LastName { get; set; }
    [MaxLength(500)]
    public required string PasswordHash { get; set; }
    public required UserType Type { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public List<Event>? Events { get; } = [];
}