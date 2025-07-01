using Domain.Core.Interfaces;
using Domain.Core.Types;

namespace Domain.OperationTasks;

public class OperationTaskParserFactory
{
    public ILogfileParser<OperationTask> Create(OperationTaskType operationTaskType, LogfileType logfileType)
    {
        return operationTaskType switch
        {
            OperationTaskType.Manufacturing => logfileType switch
            {
                LogfileType.TriDefault => new TriManufacturingOperationTaskParser(),
                _ => throw new NotImplementedException()
            },
            OperationTaskType.AddLogfileToSfc => CreateSfcResponseOperationTaskParser(logfileType),
            _ => throw new NotImplementedException()
        };
    }

    public SfcResponseOperationTaskParser CreateSfcResponseOperationTaskParser(LogfileType logfileType)
    {
        return logfileType switch
        {
            _ => new SfcResponseOperationTaskParser()
        };
    }
}