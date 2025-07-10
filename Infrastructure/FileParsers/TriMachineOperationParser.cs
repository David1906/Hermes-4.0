using Common;
using Core.Domain;
using System.Text.RegularExpressions;
using Common.ResultOf;
using Core.Application.Common.Errors;
using Infrastructure.Data.Features.Operations;

namespace Infrastructure.FileParsers;

public class TriMachineOperationParser(IResilientFileSystem fileSystem) : ILogfileParser<Operation>
{
    private const RegexOptions RgxOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
    private static readonly Regex RegexIsFail = new(@"^fail[\r\n]+", RgxOptions);

    public async Task<Operation> ParseAsync(Logfile logfile)
    {
        var content = await fileSystem.ReadAllTextAsync(logfile.FileInfo.FullName);
        return new Operation()
        {
            Type = OperationType.MachineOperation,
            Error = this.ParseResult(content),
            Logfile = logfile,
        };
    }

    private Error? ParseResult(string content)
    {
        return RegexIsFail.IsMatch(content)
            ? new InvalidDataError()
            : null;
    }
}