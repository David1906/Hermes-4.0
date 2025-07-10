using System.ComponentModel.DataAnnotations;
using Infrastructure.Data.Features.Boards;
using Infrastructure.Data.Features.Operations;

namespace Infrastructure.Data.Features.Panels;

public record PanelDbModel
{
    public int Id { get; init; }
    [MaxLength(255)] public string Model { get; set; } = "";
    public List<BoardDbModel> Boards { get; set; } = [];
    public List<OperationDbModel> Operations { get; set; } = [];
}