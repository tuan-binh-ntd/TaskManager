namespace TaskManager.Core.DomainErrors;

public sealed class UserDomainError
{
    public static Error NotFound => new("404", "Not found to user");
    public static Error ChangePasswordFailure => new("400", "Change password failure");
    public static Error InvalidEmail => new("400", "Invalid email");
    public static Error IncorrectEmailOrPassword => new("400", "Incorrect email or password");
    public static Error EmailTaken => new("400", "Email taken");
}
