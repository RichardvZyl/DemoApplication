namespace DemoApplication.Validator;

/// <summary> add notification model validator </summary>
public sealed class AddNotificationModelValidator : NotificationModelValidator
{
    public AddNotificationModelValidator()
    {
        //RuleForId(); //Generated on Add
        RuleForOriginator();
        RuleForDescription();
        RuleForSeverity();
        RuleForDate();
        RuleForForRole();
        RuleForEntity();
        RuleForRelatedId();
    }
}
