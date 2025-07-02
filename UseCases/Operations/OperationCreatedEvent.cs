using Paramore.Brighter;

namespace UseCases.Operations;

public class OperationCreatedEvent(
    string mainSerialNumber
) : Event(Guid.NewGuid())
{
    public string MainSerialNumber => mainSerialNumber;
}