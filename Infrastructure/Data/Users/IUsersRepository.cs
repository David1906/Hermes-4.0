using Domain.Users;
using ROP;

namespace Data.Data.Users;

public interface IUsersRepository
{
    Task<Result<User>> AddAsync(AddUserRequest request, CancellationToken ct = default);
}