using Common.ResultOf;
using Core.Domain;

namespace Core.Application.Common.Data;

public interface IPanelsRepository
{
    public Task<ResultOf<Panel>> AddAsync(Panel panel, CancellationToken ct = default);
    Task<ResultOf<Panel>> FindByIdAsync(int id);
}