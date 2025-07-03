using Common.Extensions;
using Common.ResultOf;
using Desktop.Core.Types;
using Desktop.Features.Logfiles.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace Desktop.Features.OperationTasks.Domain;

public class SfcResponseOperationTaskParser : ILogfileParser<OperationTask>
{
    private const RegexOptions RgxOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
    private static readonly Regex RegexWrongStation = new(@"^go-.+[\r\n]+", RgxOptions);
    private static readonly Regex RegexIsOk = new(@"^ok[\r\n]+", RgxOptions);
    private static readonly Regex RegexIsEndOfFileError = new(@"end-of-file", RgxOptions);

    private string _okResponses = "OK";
    private List<Error> _errors = [];

    public OperationTask Parse(Logfile? logfile)
    {
        return new OperationTask()
        {
            Type = OperationTaskType.AddLogfileToSfc,
            Logfile = logfile,
            Result = logfile is null
                ? ExtractResultFromErrors()
                : ExtractResult(logfile),
            Message = _errors.JoinWithNewLine()
        };
    }

    private OperationTaskResultType ExtractResult(Logfile? responseLogfile)
    {
        if (responseLogfile == null)
        {
            return OperationTaskResultType.Unknown;
        }

        if (RegexIsOk.Match(responseLogfile.Content).Success ||
            this.ContainsAdditionalOkSfcResponse(responseLogfile.Content, _okResponses))
        {
            return OperationTaskResultType.Pass;
        }

        if (RegexWrongStation.Match(responseLogfile.Content).Success)
        {
            return OperationTaskResultType.WrongStation;
        }

        if (RegexIsEndOfFileError.Match(responseLogfile.Content).Success)
        {
            return OperationTaskResultType.EndOfFile;
        }

        return OperationTaskResultType.Fail;
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

    private OperationTaskResultType ExtractResultFromErrors()
    {
        if (_errors.Any(x => x == Error.Timeout))
        {
            return OperationTaskResultType.TimedOut;
        }

        return OperationTaskResultType.Unknown;
    }

    public SfcResponseOperationTaskParser SetOkResponses(string okResponses)
    {
        _okResponses = okResponses;
        return this;
    }

    public SfcResponseOperationTaskParser SetErrors(IEnumerable<Error> resultErrors)
    {
        this._errors = resultErrors.ToList();
        return this;
    }
}