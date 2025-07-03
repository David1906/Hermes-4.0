using System.ComponentModel.DataAnnotations;

namespace Desktop.Features.Boards.Infrastructure;

public class BoardDbModel
{
    [Key] public int Id { get; set; }
    [StringLength(124)] public string SerialNumber { get; set; } = "";
}