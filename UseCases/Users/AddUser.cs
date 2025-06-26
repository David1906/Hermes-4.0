using Common.ResultOf;
using Domain.Users;
using Infrastructure.Data.Users;

namespace UseCases.Users;

public class AddUser(IUsersRepository usersRepository)
{
    public async Task<ResultOf<UserDto>> ExecuteAsync(AddUserRequest request, CancellationToken ct = default)
    {
        var isValid = Common.Validator.TryValidate(request.ToDto(), out var validationResults);
        if (!isValid)
        {
            var errors = validationResults
                .Select(r => new Error(r?.ErrorMessage ?? "Unknown validation error."))
                .ToList();
            return ResultOf<UserDto>.Failure([..errors]);
        }

        return await usersRepository
            .AddAsync(request, ct)
            .Map(x => x.ToDto());
    }
}