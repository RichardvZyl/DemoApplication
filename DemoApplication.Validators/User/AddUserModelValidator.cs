namespace DemoApplication.Validator;

/// <summary> add user model validator </summary>
public sealed class AddUserModelValidator : UserModelValidator
{
    public AddUserModelValidator()
    {
        RuleForName();
        RuleForSurname();
        RuleForEmail();
        RuleForAuth();
    }
}
