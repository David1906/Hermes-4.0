using Domain.Operations;
using Riok.Mapperly.Abstractions;

namespace UseCases.Operations;

[Mapper]
public static partial class OperationMappers
{
    public static partial OperationDto ToDto(this Operation operation);
}