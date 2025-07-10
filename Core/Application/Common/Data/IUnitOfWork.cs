namespace Core.Application.Common.Data;

public interface IUnitOfWork
{
    IPanelsRepository PanelsRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken);
}