using Core.Domain;
using Infrastructure.Data.Features.Boards;
using Infrastructure.Data.Features.Operations;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Panels;

[Mapper]
public static partial class PanelMappers
{
    [MapperIgnoreSource(nameof(Panel.MainSerialNumber))]
    [MapperIgnoreSource(nameof(Panel.ContainsFailedBoard))]
    public static partial PanelDto ToDto(this Panel panel);

    private static BoardDto ToDto(Board board) => board.ToDto();
    private static OperationDto ToDto(Operation operation) => operation.ToDto();

    public static partial Panel ToDomainModel(this PanelDto panelDto);

    private static Board ToDomainModel(BoardDto boardDto) => boardDto.ToDomainModel();
    private static Operation ToDomainModel(OperationDto operationDto) => operationDto.ToDomainModel();
}