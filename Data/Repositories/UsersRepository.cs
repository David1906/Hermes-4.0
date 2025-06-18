using Domain.Users;
using ROP;

namespace Data.Repositories;

public class UsersRepository : IUsersRepository
{
    public async Task<Result<User>> AddAsync(AddUserRequest request, CancellationToken ct = default)
    {
        return new User
        {
            Email = request.Email,
            Name = request.Name,
            Age = request.Age
        };
    }
}