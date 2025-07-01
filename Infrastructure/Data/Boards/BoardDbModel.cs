using System.ComponentModel.DataAnnotations;
using Infrastructure.Data.Defects;

namespace Infrastructure.Data.Boards;

public class BoardDbModel
{
    [Key] public int Id { get; set; }
    [StringLength(124)] public string SerialNumber { get; set; } = "";
}