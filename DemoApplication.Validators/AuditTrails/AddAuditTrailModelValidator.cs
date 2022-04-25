namespace DemoApplication.Validator;

public sealed class AddAuditTrailModelValidator : AuditTrailModelValidator
{
    public AddAuditTrailModelValidator()
    {
        RuleForId();
        RuleForContents();
        RuleForDate();
        RuleForDisplayContext();
        RuleForModel();
        RuleForUserId();
    }
}
