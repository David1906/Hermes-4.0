using System.ComponentModel.DataAnnotations;

namespace Desktop.Features.Users.Infrastructure;

public class UserDbModel
{
    [Key] public int Id { get; set; }
    [StringLength(124)] public string Name { get; set; } = "";
    [StringLength(124)] public string LastName { get; set; } = "";
    [StringLength(124)] public string Email { get; set; } = "";
}