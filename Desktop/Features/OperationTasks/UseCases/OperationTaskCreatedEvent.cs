using System;
using Desktop.Features.OperationTasks.Domain;
using Paramore.Brighter;

namespace Desktop.Features.OperationTasks.UseCases;

public class OperationTaskCreatedEvent(
    string mainSerialNumber,
    OperationTask operationTask)
    : Event(Guid.NewGuid())
{
    public string MainSerialNumber { get; } = mainSerialNumber;
    public OperationTask OperationTask => operationTask;
}