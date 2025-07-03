using Desktop.Features.Boards.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Desktop.Features.Panels.Infrastructure;

public class PanelDbModel
{
    [Key] public int Id { get; set; }
    public required List<BoardDbModel> Boards { get; init; }
}