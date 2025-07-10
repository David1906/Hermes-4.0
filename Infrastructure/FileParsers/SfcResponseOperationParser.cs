using Common.ResultOf;
using Common;
using Core.Domain;
using System.Text.RegularExpressions;

namespace Infrastructure.FileParsers;

public class SfcResponseOperationParser(IResilientFileSystem fileSystem)
{
    private const RegexOptions RgxOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
    private static readonly Regex RegexWrongStation = new(@"^go-.+[\r\n]+", RgxOptions);
    private static readonly Regex RegexIsOk = new(@"^ok[\r\n]+", RgxOptions);
    private static readonly Regex RegexIsEndOfFileError = new(@"end-of-file", RgxOptions);

    public async Task<Operation> ParseAsync(Logfile logfile, string okResponses)
    {
        var content = await fileSystem.ReadAllTextAsync(logfile.FileInfo.FullName);
        return new Operation()
        {
            Type = OperationType.SendPanelToNextStation,
            Logfile = logfile,
            Result = ExtractResult(content, okResponses),
        };
    }

    public Operation Parse(IEnumerable<Error> errors)
    {
        return new Operation()
        {
            Type = OperationType.SendPanelToNextStation,
            Result = ExtractResult(errors)
        };
    }

    private OperationResultType ExtractResult(string content, string okResponses)
    {
        if (string.IsNullOrEmpty(content))
        {
            return OperationResultType.Unknown;
        }

        if (RegexIsOk.Match(content).Success ||
            this.ContainsAdditionalOkSfcResponse(content, okResponses))
        {
            return OperationResultType.Pass;
        }

        if (RegexWrongStation.Match(content).Success)
        {
            return OperationResultType.WrongStation;
        }

        if (RegexIsEndOfFileError.Match(content).Success)
        {
            return OperationResultType.EndOfFile;
        }

        return OperationResultType.Fail;
    }

    private bool ContainsAdditionalOkSfcResponse(string content, string okResponses)
    {
        var responses = okResponses
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim().ToLower());
        var lowerContent = content.ToLower();

        foreach (var additionalResponse in responses)
        {
            if (lowerContent.Contains(additionalResponse))
            {
                return true;
            }
        }

        return false;
    }

    private OperationResultType ExtractResult(IEnumerable<Error> errors)
    {
        if (errors.Any(x => x == Error.Timeout))
        {
            return OperationResultType.TimedOut;
        }

        return OperationResultType.Unknown;
    }
}