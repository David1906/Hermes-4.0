using Core.Domain;
using Infrastructure.Data.Features.Logfiles;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Features.Operations;

[Mapper]
public static partial class OperationMappers
{
    [MapperIgnoreSource(nameof(Operation.IsFailure))]
    [MapperIgnoreSource(nameof(Operation.Title))]
    public static partial OperationDbModel ToDbModel(this Operation operation);

    private static LogfileDbModel ToDbModel(Logfile logfile) => logfile.ToDbModel();

    public static partial Operation ToDomainModel(this OperationDbModel operationDbModel);
}