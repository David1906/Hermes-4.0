using Common.ResultOf;
using Common;
using Core.Application.Common.Errors;
using Core.Application.Common.FileParsers;
using Core.Domain;
using System.Text.RegularExpressions;

namespace Infrastructure.FileParsers;

public class PanelParser(IResilientFileSystem fileSystem) : IPanelParser
{
    private const string GoodDefectText = "GOOD";
    private const RegexOptions RgxOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
    private static readonly Regex DefectRgx = new($"({GoodDefectText}|BAD);(.*);(.*);(.*);(.*);", RgxOptions);
    private static readonly Regex SerialNumberRgx = new(@"^\s*([A-z0-9-_]+)[\r\n]+");
    private static readonly Regex PassRgx = new(@"^PASS$", RgxOptions);

    public async Task<ResultOf<Panel>> ParseAsync(Logfile logfile, CancellationToken cancellationToken)
    {
        var content = await fileSystem.ReadAllTextAsync(logfile.FileInfo.FullName, cancellationToken);
        var boards = ParseBoards(content);
        if (boards.Count == 0)
        {
            return ResultOf<Panel>.Failure(new InvalidDataError());
        }

        return new Panel(boards)
        {
            Model = this.ParseModel(content)
        };
    }

    private string ParseModel(string content)
    {
        using var reader = new StringReader(content);
        for (var i = 0; i < 3; i++)
        {
            reader.ReadLine();
        }

        return reader.ReadLine() ?? string.Empty;
    }

    private List<Board> ParseBoards(string content)
    {
        var matches = SerialNumberRgx.Matches(content);
        var boards = new List<Board>();
        var isPass = PassRgx.IsMatch(content);
        foreach (var match in matches.Cast<Match>())
        {
            if (match.Captures.Count > 0)
            {
                boards.Add(new Board
                {
                    IsPass = isPass,
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
}