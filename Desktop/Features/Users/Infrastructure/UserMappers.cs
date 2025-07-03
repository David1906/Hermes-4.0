using Desktop.Features.Users.Domain;
using Riok.Mapperly.Abstractions;

namespace Desktop.Features.Users.Infrastructure;

[Mapper]
public static partial class UserMappers
{
    public static partial UserDbModel ToDbModel(this User user);
}