using Common.ResultOf;
using Core.Domain;

namespace Core.Application.Common.FileParsers;

public interface IOperationParser
{
    Task<ResultOf<Operation>> ParseMachineOperationAsync(Logfile logfile);
    Task<Operation> ParseSfcResponseAsync(Logfile? logfile, IEnumerable<Error> errors, string okResponses);
}