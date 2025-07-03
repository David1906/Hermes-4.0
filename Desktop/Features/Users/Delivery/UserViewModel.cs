using CommunityToolkit.Mvvm.Input;
using Desktop.Common;
using Desktop.Features.Users.Domain;
using Desktop.Features.Users.UseCases;
using System.Threading.Tasks;

namespace Desktop.Features.Users.Delivery;

public partial class UserViewModel(
    UsersUseCases usersUseCases
) : ViewModelBase
{
    public string Name { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";

    [RelayCommand]
    private async Task AddUser()
    {
        await usersUseCases.AddUser.ExecuteAsync(new AddUserCommand
        (
            Name = Name,
            LastName = LastName,
            Email = Email
        ));
    }
}