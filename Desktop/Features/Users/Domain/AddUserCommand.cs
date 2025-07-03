namespace Desktop.Features.Users.Domain;

public record AddUserCommand(
    string Name,
    string LastName,
    string Email
);