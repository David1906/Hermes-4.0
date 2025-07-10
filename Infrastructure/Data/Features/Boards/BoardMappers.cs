using Core.Domain;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Boards;

[Mapper]
public static partial class BoardMappers
{
    [MapperIgnoreSource(nameof(Board.IsPass))]
    public static partial BoardDbModel ToDbModel(this Board board);

    [MapperIgnoreTarget(nameof(Board.IsPass))]
    public static partial Board ToDomainModel(this BoardDbModel boardDbModel);
}