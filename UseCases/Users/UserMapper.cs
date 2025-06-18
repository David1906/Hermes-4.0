using Domain.Users;

namespace UseCases.Users;

public static class UserMapper
{
    public static UserDto ToDto(this AddUserRequest request)
    {
        return new UserDto
        {
            Email = request.Email,
            Name = request.Name,
            Age = request.Age
        };
    }

    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Email = user.Email,
            Name = user.Name,
            Age = user.Age
        };
    }
}