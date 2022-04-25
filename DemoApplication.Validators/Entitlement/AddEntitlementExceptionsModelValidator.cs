namespace DemoApplication.Validator;

public sealed class AddEntitlementExceptionsModelValidator : EntitlementExceptionsModelValidator
{
    public AddEntitlementExceptionsModelValidator() 
        => RuleForUserId();
}
