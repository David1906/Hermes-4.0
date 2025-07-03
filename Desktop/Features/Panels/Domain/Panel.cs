using Desktop.Features.Boards.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Desktop.Features.Panels.Domain;

public record Panel
{
    public int Id { get; set; }
    public required List<Board> Boards { get; init; } = [];
    public string MainSerialNumber => Boards.First().SerialNumber;
}