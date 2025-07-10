using Common.ResultOf;
using Core.Application.Common.Data;
using Core.Application.Common.Errors;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Features.Panels;

public class PanelsRepository(HermesContext hermesContext) : IPanelsRepository
{
    public async Task<ResultOf<Panel>> AddAsync(Panel panel, CancellationToken ct)
    {
        var dbModel = panel.ToDbModel();
        await hermesContext.Panels.AddAsync(dbModel, ct);
        return dbModel.ToDomainModel();
    }

    public async Task<ResultOf<Panel>> FindByIdAsync(int id)
    {
        var panel = await hermesContext.Panels
            .Where(x => x.Id == id)
            .Include(x => x.Boards)
            .Include(x => x.Operations)
            .Select(x => x.ToDomainModel())
            .FirstOrDefaultAsync();
        return panel ?? ResultOf<Panel>.Failure(new NotFoundError());
    }
}