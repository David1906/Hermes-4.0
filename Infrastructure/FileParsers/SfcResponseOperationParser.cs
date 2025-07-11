using Common.ResultOf;
using Common;
using Core.Application.Common.Errors;
using Core.Domain;
using System.Text.RegularExpressions;
using Core.Application.Common.FileParsers;

namespace Infrastructure.FileParsers;

public class SfcResponseOperationParser(IResilientFileSystem fileSystem) : ISfcResponseOperationParser
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
            Error = ExtractResult(content, okResponses),
        };
    }

    public Operation Parse(IEnumerable<Error> errors)
    {
        return new Operation()
        {
            Type = OperationType.SendPanelToNextStation,
            Error = errors.First()
        };
    }

    public async Task<Error?> ParseErrorAsync(Logfile logfile, string okResponses)
    {
        var content = await fileSystem.ReadAllTextAsync(logfile.FileInfo.FullName);
        return ExtractResult(content, okResponses);
    }

    private Error? ExtractResult(string content, string okResponses)
    {
        if (string.IsNullOrEmpty(content))
        {
            return new UnknownError();
        }

        if (RegexIsOk.Match(content).Success ||
            this.ContainsAdditionalOkSfcResponse(content, okResponses))
        {
            return null;
        }

        if (RegexWrongStation.Match(content).Success)
        {
            return new WrongStationError(content);
        }

        if (RegexIsEndOfFileError.Match(content).Success)
        {
            return new EndOfFileError(content);
        }

        return new UnknownError();
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
}