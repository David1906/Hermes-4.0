using System.Text.RegularExpressions;
using Domain.Boards;
using Domain.Core.Types;
using Domain.Defects;
using Domain.Logfiles;

namespace Domain.Operations;

public class TriOperationParser
{
    private const string GoodDefectText = "GOOD";
    private const RegexOptions RgxOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
    private static readonly Regex DefectRgx = new($"({GoodDefectText}|BAD);(.*);(.*);(.*);(.*);", RgxOptions);
    private static readonly Regex RegexIsFail = new(@"^fail[\r\n]+", RgxOptions);
    private static readonly Regex SerialNumberRgx = new(@"^\s*([A-z0-9-_]+)[\r\n]+");

    public Operation Parse(string content, string path)
    {
        return this.Parse(new Logfile
        {
            Content = content,
            FileInfo = new FileInfo(path)
        });
    }

    public Operation Parse(Logfile logfile)
    {
        return new Operation()
        {
            Boards = this.ParseBoards(logfile.Content),
            Result = this.ParseOperationResult(logfile.Content),
            Logfile = logfile
        };
    }

    private List<Board> ParseBoards(string content)
    {
        var matches = SerialNumberRgx.Matches(content);
        var boards = new List<Board>();
        foreach (var match in matches.Cast<Match>())
        {
            if (match.Captures.Count > 0)
            {
                boards.Add(new Board
                {
                    SerialNumber = match.Groups[1].Value,
                    Defects = this.ParseDefects(content)
                });
            }
        }

        return boards;
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

    private OperationResultType ParseOperationResult(string content)
    {
        return RegexIsFail.IsMatch(content)
            ? OperationResultType.Fail
            : OperationResultType.Pass;
    }
}