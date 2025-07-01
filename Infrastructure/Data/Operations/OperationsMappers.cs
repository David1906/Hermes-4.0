using Domain.OperationTasks;
using Domain.Operations;
using Domain.Panels;
using Infrastructure.Data.OperationTasks;
using Infrastructure.Data.Panels;
using Riok.Mapperly.Abstractions;

namespace Infrastructure.Data.Operations;

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
        => logfileDbModel.Select(x => x.ToDbModel()).ToList();

    [MapperIgnoreSource(nameof(Panel.MainSerialNumber))]
    public static partial PanelDbModel ToDbModel(this Panel panel);

    [MapProperty(
        nameof(OperationDbModel.Tasks),
        nameof(Operation.Tasks),
        Use = nameof(OperationTaskFromDbModelToDomainModel))]
    public static partial Operation ToDomainModel(this OperationDbModel operationDbModel);

    private static List<OperationTask> OperationTaskFromDbModelToDomainModel(List<OperationTaskDbModel> logfileDbModel)
        => logfileDbModel.Select(x => x.ToDomainModel()).ToList();
}