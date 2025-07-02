using Domain.OperationTasks;
using Paramore.Brighter;

namespace UseCases.OperationTasks;

public class OperationTaskCreatedEvent(
    string mainSerialNumber,
    OperationTask operationTask)
    : Event(Guid.NewGuid())
{
    public string MainSerialNumber { get; } = mainSerialNumber;
    public OperationTask OperationTask => operationTask;
}