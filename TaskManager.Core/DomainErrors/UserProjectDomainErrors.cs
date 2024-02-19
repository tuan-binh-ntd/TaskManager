namespace TaskManager.Core.DomainErrors;

public sealed class UserProjectDomainErrors
{
    public static Error CanNotInsert => new("400", "UserIds can't be empty");
}
