using Domain.Core.Errors;
using Domain.Users;
using Infrastructure.Data.Users;
using OneOf;

namespace UseCases.Users;

public class AddUser(IUsersRepository usersRepository)
{
    public async Task<OneOf<UserDto, Error>> ExecuteAsync(AddUserRequest request, CancellationToken ct = default)
    {
        var isValid = Common.Validator.TryValidate(request.ToDto(), out var validationResults);
        if (!isValid)
        {
            var errors = validationResults.Select(r => new Error(r.ErrorMessage));
            return new BadRequestError([..errors]);
        }

        var result = await usersRepository.AddAsync(request, ct);
        return result.MapT0(x => x.ToDto());
    }
}