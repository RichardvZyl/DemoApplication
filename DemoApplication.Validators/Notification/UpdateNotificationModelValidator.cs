namespace DemoApplication.Validator;

public abstract class UpdateNotificationModelValidator : NotificationModelValidator
{
    public UpdateNotificationModelValidator()
    {
        RuleForId();
        RuleForOriginator();
        RuleForDescription();
        RuleForSeverity();
        RuleForDate();
        //RuleForSeenBy();
        //RuleForSeenAt();
    }
}
