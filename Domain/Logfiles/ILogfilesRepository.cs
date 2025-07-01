using Common.ResultOf;

namespace Domain.Logfiles;

public interface ILogfilesRepository
{
    Task<ResultOf<Logfile>> AddAsync(Logfile logfile, CancellationToken ct = default);
    Task<ResultOf<Logfile>> GetByIdAsync(int id, CancellationToken ct);
}