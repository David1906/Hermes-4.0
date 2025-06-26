using Common.ResultOf;
using Domain.Users;

namespace Infrastructure.Data.Users;

public class UsersRepository : IUsersRepository
{
    public async Task<ResultOf<User>> AddAsync(AddUserRequest request, CancellationToken ct = default)
    {
        return new User
        {
            Email = request.Email,
            Name = request.Name,
            Age = request.Age
        };
    }
}