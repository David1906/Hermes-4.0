using Common.ResultOf;
using Domain.Users;

namespace Infrastructure.Data.Users;

public interface IUsersRepository
{
    Task<ResultOf<User>> AddAsync(AddUserRequest request, CancellationToken ct = default);
}