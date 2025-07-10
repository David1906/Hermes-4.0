using Core.Domain;
using Infrastructure.Data.Features.Boards;
using Infrastructure.Data.Features.Logfiles;
using Infrastructure.Data.Features.Operations;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Panels;

[Mapper]
public static partial class PanelMappers
{
    [MapperIgnoreSource(nameof(Panel.MainSerialNumber))]
    [MapperIgnoreSource(nameof(Panel.ContainsFailedBoard))]
    public static partial PanelDbModel ToDbModel(this Panel panel);

    private static BoardDbModel ToDbModel(Board board) => board.ToDbModel();
    private static OperationDbModel ToDbModel(Operation operation) => operation.ToDbModel();

    public static partial Panel ToDomainModel(this PanelDbModel panelDbModel);

    private static Board ToDomainModel(BoardDbModel boardDbModel) => boardDbModel.ToDomainModel();
}