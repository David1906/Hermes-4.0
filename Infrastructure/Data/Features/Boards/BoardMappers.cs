using Core.Domain;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Boards;

[Mapper]
public static partial class BoardMappers
{
    [MapperIgnoreSource(nameof(Board.IsPass))]
    public static partial BoardDto ToDto(this Board board);

    [MapperIgnoreTarget(nameof(Board.IsPass))]
    public static partial Board ToDomainModel(this BoardDto boardDto);
}