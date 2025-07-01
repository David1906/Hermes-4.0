using Domain.Logfiles;

namespace Domain.Core.Interfaces;

public interface ILogfileParser<T> where T : class
{
    T Parse(Logfile? logfile);
}