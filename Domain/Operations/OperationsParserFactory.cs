using Domain.Core.Interfaces;
using Domain.Core.Types;

namespace Domain.Operations;

public class OperationsParserFactory
{
    public ILogfileParser<Operation> Create(LogfileType logfileType)
    {
        return logfileType switch
        {
            _ => new TriOperationParser()
        };
    }
}