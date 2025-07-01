using Infrastructure.Data.Boards;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Panels;

public class PanelDbModel
{
    [Key] public int Id { get; set; }
    public required List<BoardDbModel> Boards { get; init; }
}