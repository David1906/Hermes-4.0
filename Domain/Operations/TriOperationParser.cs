using Domain.Boards;
using Domain.Logfiles;
using Domain.Core.Interfaces;
using Domain.Panels;
using System.Text.RegularExpressions;

namespace Domain.Operations;

public class TriOperationParser : ILogfileParser<Operation>
{
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
            Panel = new Panel()
            {
                Boards = this.ParseBoards(logfile.Content)
            }
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
                });
            }
        }

        return boards;
    }
}