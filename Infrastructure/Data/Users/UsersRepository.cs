using Domain.Core.Errors;
using Domain.Users;
using OneOf;

namespace Infrastructure.Data.Users;

public class UsersRepository : IUsersRepository
{
    public async Task<OneOf<User, Error>> AddAsync(AddUserRequest request, CancellationToken ct = default)
    {
        return new User
        {
            Email = request.Email,
            Name = request.Name,
            Age = request.Age
        };
    }
}