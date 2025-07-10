using Common.ResultOf;
using Core.Domain;

namespace Core.Application.Common.Data;

public interface ILogfilesRepository
{
    Task<ResultOf<Logfile>> AddAsync(Logfile logfile, CancellationToken ct = default);
    Task<ResultOf<Logfile>> GetByIdAsync(int id, CancellationToken ct);
}