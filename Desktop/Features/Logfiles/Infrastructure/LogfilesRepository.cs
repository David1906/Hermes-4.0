using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ResultOf;
using Desktop.Data;
using Desktop.Features.Logfiles.Domain;

namespace Desktop.Features.Logfiles.Infrastructure;

public class LogfilesRepository(SqliteContext context) : ILogfilesRepository
{
    public async Task<ResultOf<Logfile>> AddAsync(Logfile logfile, CancellationToken ct = default)
    {
        try
        {
            var dbModel = logfile.ToDbModel();
            context.Logfiles.Add(dbModel);
            await context.SaveChangesAsync(ct);
            var newLogfile = dbModel.ToDomainModel();
            newLogfile.Content = logfile.Content;
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
            var dbModel = await context.Logfiles.FindAsync([id], cancellationToken: ct);
            return dbModel?.ToDomainModel() ?? ResultOf<Logfile>.Failure("Logfile not found.");
        }
        catch (Exception e)
        {
            return ResultOf<Logfile>.Failure(e.Message);
        }
    }
}