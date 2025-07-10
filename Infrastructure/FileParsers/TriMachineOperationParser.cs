using Common;
using Core.Domain;
using System.Text.RegularExpressions;
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
            Result = this.ParseResult(content),
            Logfile = logfile,
        };
    }

    private OperationResultType ParseResult(string content)
    {
        return RegexIsFail.IsMatch(content)
            ? OperationResultType.Fail
            : OperationResultType.Pass;
    }
}