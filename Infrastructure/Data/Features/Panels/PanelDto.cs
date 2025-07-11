using System.ComponentModel.DataAnnotations;
using Infrastructure.Data.Features.Boards;
using Infrastructure.Data.Features.Operations;

namespace Infrastructure.Data.Features.Panels;

public record PanelDto
{
    public int Id { get; init; }
    [MaxLength(255)] public string Model { get; set; } = "";
    public List<BoardDto> Boards { get; set; } = [];
    public List<OperationDto> Operations { get; set; } = [];
}