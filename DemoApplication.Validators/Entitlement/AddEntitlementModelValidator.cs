namespace DemoApplication.Validator;

public sealed class AddEntitlementModelValidator : EntitlementModelValidator
{
    public AddEntitlementModelValidator()
        => RuleForUserId();
}
