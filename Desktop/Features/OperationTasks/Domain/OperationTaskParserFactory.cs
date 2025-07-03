using System;
using Desktop.Core.Interfaces;
using Desktop.Core.Types;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.OperationTasks.Domain;

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