using Core.Domain;
using Paramore.Brighter;

namespace Core.Application.Common.Events;

public class OperationCreatedEvent(
    Operation operation,
    string mainSerialNumber) 
    : Event(Guid.NewGuid())
{
    public Operation Operation => operation;
    public string MainSerialNumber => mainSerialNumber;
}