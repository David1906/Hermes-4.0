using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;

namespace Desktop.Features.Logfiles.Domain;

public interface ILogfilesRepository
{
    Task<ResultOf<Logfile>> AddAsync(Logfile logfile, CancellationToken ct = default);
    Task<ResultOf<Logfile>> GetByIdAsync(int id, CancellationToken ct);
}