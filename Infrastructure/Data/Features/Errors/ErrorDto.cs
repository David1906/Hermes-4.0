using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Features.Errors;

public class ErrorDto
{
    [Key] public int Id { get; set; }
    [MaxLength(255)] public string Message { get; set; } = "";
    [MaxLength(255)] public string ErrorType { get; set; } = "";
}