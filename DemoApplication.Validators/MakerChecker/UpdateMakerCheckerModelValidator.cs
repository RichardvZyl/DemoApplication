namespace DemoApplication.Validator;

public sealed class UpdateMakerCheckerModelValidator : MakerCheckerModelValidator
{
    public UpdateMakerCheckerModelValidator()
    {
        RuleForId();
        RuleForAction();
        RuleForMakerDate();
        RuleForMakerUser();
        RuleForMotivation();
        //RuleForFiles();
        RuleForCheckerDate();
        RuleForCheckerUser();
        RuleForAccepted();
    }
}
