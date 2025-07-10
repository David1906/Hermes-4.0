using Common.ResultOf;
using Core.Domain;

namespace Core.Application.Common.FileParsers;

public interface IPanelParser
{
    Task<ResultOf<Panel>> ParseAsync(Logfile logfile, CancellationToken cancellationToken);
}