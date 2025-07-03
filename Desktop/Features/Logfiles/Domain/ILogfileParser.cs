namespace Desktop.Features.Logfiles.Domain;

public interface ILogfileParser<T> where T : class
{
    T Parse(Logfile? logfile);
}