using Desktop.Features.OperationTasks.Domain;
using Desktop.Features.OperationTasks.Infrastructure;
using Desktop.Features.Operations.Domain;
using Desktop.Features.Panels.Domain;
using Desktop.Features.Panels.Infrastructure;
using Riok.Mapperly.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Desktop.Features.Operations.Infrastructure;

[Mapper]
public static partial class OperationsMappers
{
    [MapperIgnoreSource(nameof(Operation.MainSerialNumber))]
    [MapProperty(
        nameof(Operation.Tasks),
        nameof(OperationDbModel.Tasks),
        Use = nameof(OperationTaskFromDomainModelToDbModel))]
    public static partial OperationDbModel ToDbModel(this Operation operation);

    private static List<OperationTaskDbModel> OperationTaskFromDomainModelToDbModel(List<OperationTask> logfileDbModel)
        => logfileDbModel.Select(x => OperationTasksMapper.ToDbModel((OperationTask)x)).ToList();

    [MapperIgnoreSource(nameof(Panel.MainSerialNumber))]
    public static partial PanelDbModel ToDbModel(this Panel panel);

    [MapProperty(
        nameof(OperationDbModel.Tasks),
        nameof(Operation.Tasks),
        Use = nameof(OperationTaskFromDbModelToDomainModel))]
    public static partial Operation ToDomainModel(this OperationDbModel operationDbModel);

    private static List<OperationTask> OperationTaskFromDbModelToDomainModel(List<OperationTaskDbModel> logfileDbModel)
        => logfileDbModel.Select<OperationTaskDbModel, OperationTask>(x => x.ToDomainModel()).ToList();
}