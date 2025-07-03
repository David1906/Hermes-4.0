using Desktop.Core.Types;
using Desktop.Features.Defects.Domain;
using Desktop.Features.Logfiles.Domain;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Desktop.Features.OperationTasks.Domain;

public class TriManufacturingOperationTaskParser : ILogfileParser<OperationTask>
{
    private const string GoodDefectText = "GOOD";
    private const RegexOptions RgxOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
    private static readonly Regex DefectRgx = new($"({GoodDefectText}|BAD);(.*);(.*);(.*);(.*);", RgxOptions);
    private static readonly Regex RegexIsFail = new(@"^fail[\r\n]+", RgxOptions);

    public OperationTask Parse(string content, string path)
    {
        return this.Parse(new Logfile
        {
            Content = content,
            FileInfo = new FileInfo(path)
        });
    }

    public OperationTask Parse(Logfile? logfile)
    {
        return new OperationTask()
        {
            Type = OperationTaskType.Manufacturing,
            Defects = this.ParseDefects(logfile?.Content ?? ""),
            Result = this.ParseResult(logfile?.Content ?? ""),
        };
    }

    private List<Defect> ParseDefects(string content)
    {
        var defects = new List<Defect>();
        var matches = DefectRgx.Matches(content);
        foreach (Match match in matches)
        {
            var defect = new Defect()
            {
                ErrorFlag = match.Groups[1].Value != GoodDefectText ? ErrorFlagType.Good : ErrorFlagType.Bad,
                Location = match.Groups[4].Value,
                ErrorCode = match.Groups[5].Value
            };
            defects.Add(defect);
        }

        return defects;
    }

    private OperationTaskResultType ParseResult(string content)
    {
        return RegexIsFail.IsMatch(content)
            ? OperationTaskResultType.Fail
            : OperationTaskResultType.Pass;
    }
}