using Common.ResultOf;
using Core.Domain;

namespace Core.Application.Common.FileParsers;

public interface ISfcResponseOperationParser
{
    Task<Operation> ParseAsync(Logfile logfile, string okResponses);
    Operation Parse(IEnumerable<Error> errors);
    Task<Error?> ParseErrorAsync(Logfile logfile, string okResponses);
}