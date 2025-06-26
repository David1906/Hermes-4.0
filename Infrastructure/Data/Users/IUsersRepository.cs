using Domain.Core.Errors;
using Domain.Users;
using OneOf;

namespace Infrastructure.Data.Users;

public interface IUsersRepository
{
    Task<OneOf<User, Error>> AddAsync(AddUserRequest request, CancellationToken ct = default);
}