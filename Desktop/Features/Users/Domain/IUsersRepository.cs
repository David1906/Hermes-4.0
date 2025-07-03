using System.Threading.Tasks;

namespace Desktop.Features.Users.Domain;

public interface IUsersRepository
{
    Task AddAsync(User user);
}