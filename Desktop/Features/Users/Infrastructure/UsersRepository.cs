using System.Threading.Tasks;
using Desktop.Data;
using Desktop.Features.Users.Domain;

namespace Desktop.Features.Users.Infrastructure;

public class UsersRepository(SqliteContext context) : IUsersRepository
{
    public async Task AddAsync(User user)
    {
        context.Users.Add(user.ToDbModel());
        await context.SaveChangesAsync();
    }
}