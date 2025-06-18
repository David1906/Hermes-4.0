using Data.Repositories;
using Domain.Users;
using ROP;

namespace UseCases.Users;

public class AddUser(IUsersRepository usersRepository)
{
    public async Task<Result<UserDto>> ExecuteAsync(AddUserRequest request, CancellationToken ct = default)
    {
        var isValid = Common.Validator.TryValidate(request.ToDto(), out var validationResults);
        if (!isValid)
        {
            var errors = validationResults.Select(r => Error.Create(r.ErrorMessage, Guid.NewGuid()));
            return Result.BadRequest<UserDto>([..errors]);
        }

        return await usersRepository
            .AddAsync(request, ct)
            .Map(x => x.ToDto());
    }
}