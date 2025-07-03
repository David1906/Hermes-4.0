using Desktop.Features.Logfiles.Domain;
using Desktop.Features.Logfiles.Infrastructure;
using Desktop.Features.OperationTasks.Domain;
using Riok.Mapperly.Abstractions;

namespace Desktop.Features.OperationTasks.Infrastructure;

[Mapper]
public static partial class OperationTasksMapper
{
    [MapProperty(
        nameof(OperationTask.Logfile),
        nameof(OperationTaskDbModel.Logfile),
        Use = nameof(MapLogfileToLogfileDbModel))]
    [MapperIgnoreSource(nameof(OperationTask.IsFailure))]
    public static partial OperationTaskDbModel ToDbModel(this OperationTask operationTask);

    private static LogfileDbModel? MapLogfileToLogfileDbModel(Logfile? logfileDbModel)
        => logfileDbModel?.ToDbModel();

    [MapProperty(
        nameof(OperationTaskDbModel.Logfile),
        nameof(OperationTask.Logfile),
        Use = nameof(MapLogfileDbModelToLogfile))]
    [MapperIgnoreTarget(nameof(OperationTask.IsFailure))]
    public static partial OperationTask ToDomainModel(this OperationTaskDbModel operationTaskDbModel);

    private static Logfile? MapLogfileDbModelToLogfile(LogfileDbModel? logfileDbModel)
        => logfileDbModel?.ToDomainModel();
}