using Domain.Boards;
using System.Collections.Immutable;

namespace Domain.Panels;

public record Panel
{
    public int Id { get; set; }
    public required List<Board> Boards { get; init; } = [];
    public string MainSerialNumber => Boards.First().SerialNumber;
}