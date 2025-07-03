using System;
using Paramore.Brighter;

namespace Desktop.Features.Operations.UseCases;

public class OperationCreatedEvent(
    string mainSerialNumber
) : Event(Guid.NewGuid())
{
    public string MainSerialNumber => mainSerialNumber;
}