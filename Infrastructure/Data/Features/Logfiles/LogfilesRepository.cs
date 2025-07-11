using Common.ResultOf;
using Core.Application.Common.Data;
using Core.Domain;

namespace Infrastructure.Data.Features.Logfiles;

public class LogfilesRepository(HermesContext hermesContext) : ILogfilesRepository
{
    public async Task<ResultOf<Logfile>> AddAsync(Logfile logfile, CancellationToken ct = default)
    {
        try
        {
            var dbModel = logfile.ToDto();
            hermesContext.Logfiles.Add(dbModel);
            await hermesContext.SaveChangesAsync(ct);
            var newLogfile = dbModel.ToDomainModel();
            return newLogfile;
        }
        catch (Exception e)
        {
            return ResultOf<Logfile>.Failure($"Error saving logfile {logfile.Name} - " + e.Message);
        }
    }

    public async Task<ResultOf<Logfile>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var dbModel = await hermesContext.Logfiles.FindAsync([id], cancellationToken: ct);
            return dbModel?.ToDomainModel() ?? ResultOf<Logfile>.Failure("Logfile not found.");
        }
        catch (Exception e)
        {
            return ResultOf<Logfile>.Failure(e.Message);
        }
    }
}