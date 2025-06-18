using Domain.Users;
using ROP;

namespace Data.Repositories;

public interface IUsersRepository
{
    Task<Result<User>> AddAsync(AddUserRequest request, CancellationToken ct = default);
}