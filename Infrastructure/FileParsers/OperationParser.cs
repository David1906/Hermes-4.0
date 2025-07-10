using Common.ResultOf;
using Core.Application.Common.FileParsers;
using Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.FileParsers;

public class OperationParser(IServiceProvider serviceProvider) : IOperationParser
{
    public async Task<ResultOf<Operation>> ParseMachineOperationAsync(Logfile logfile)
    {
        var parser = logfile.Type switch
        {
            _ => serviceProvider.GetRequiredService<TriMachineOperationParser>()
        };
        return await parser.ParseAsync(logfile);
    }

    public async Task<Operation> ParseSfcResponseAsync(
        Logfile? logfile,
        IEnumerable<Error> errors,
        string okResponses)
    {
        var parser = serviceProvider.GetRequiredService<SfcResponseOperationParser>();
        if (logfile is null)
        {
            return parser.Parse(errors);
        }

        return await parser.ParseAsync(logfile, okResponses);
    }
}