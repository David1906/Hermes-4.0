using Domain.Logfiles;
using Domain.OperationTasks;
using Infrastructure.Data.Logfiles;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.OperationTasks;

[Mapper]
public static partial class OperationTasksMapper
{
    [MapProperty(
        nameof(OperationTask.Logfile),
        nameof(OperationTaskDbModel.Logfile),
        Use = nameof(MapLogfileToLogfileDbModel))]
    public static partial OperationTaskDbModel ToDbModel(this OperationTask operationTask);

    private static LogfileDbModel? MapLogfileToLogfileDbModel(Logfile? logfileDbModel)
        => logfileDbModel?.ToDbModel();

    [MapProperty(
        nameof(OperationTaskDbModel.Logfile),
        nameof(OperationTask.Logfile),
        Use = nameof(MapLogfileDbModelToLogfile))]
    public static partial OperationTask ToDomainModel(this OperationTaskDbModel operationTaskDbModel);

    private static Logfile? MapLogfileDbModelToLogfile(LogfileDbModel? logfileDbModel)
        => logfileDbModel?.ToDomainModel();
}