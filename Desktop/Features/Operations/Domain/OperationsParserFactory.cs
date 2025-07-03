using Desktop.Core.Interfaces;
using Desktop.Core.Types;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.Operations.Domain;

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