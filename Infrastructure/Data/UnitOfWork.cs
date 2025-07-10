using Core.Application.Common.Data;
using Infrastructure.Data.Features.Panels;

namespace Infrastructure.Data;

public class UnitOfWork(HermesContext hermesContext) : IUnitOfWork
{
    private IPanelsRepository? _panelsRepository;
    public IPanelsRepository PanelsRepository => _panelsRepository ??= new PanelsRepository(hermesContext);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await hermesContext.SaveChangesAsync(cancellationToken);
    }
}