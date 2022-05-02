namespace DemoApplication.Validator;

/// <summary> update user model validator </summary>
public sealed class UpdateUserModelValidator : UserModelValidator
{
    public UpdateUserModelValidator()
    {
        RuleForId();
        RuleForName();
        RuleForSurname();
        RuleForEmail();
    }
}
