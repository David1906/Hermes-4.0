using Core.Domain;

namespace Infrastructure.FileParsers;

public interface ILogfileParser<T>
{
    Task<T> ParseAsync(Logfile content);
}