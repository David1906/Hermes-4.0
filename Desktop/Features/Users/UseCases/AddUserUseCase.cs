using System.Threading.Tasks;
using Desktop.Features.Users.Domain;

namespace Desktop.Features.Users.UseCases;

public class AddUserUseCase(IUsersRepository usersRepository)
{
    public async Task ExecuteAsync(AddUserCommand command)
    {
        await usersRepository.AddAsync(new User()
        {
            Name = command.Name,
            LastName = command.LastName,
            Email = command.Email
        });
    }
}