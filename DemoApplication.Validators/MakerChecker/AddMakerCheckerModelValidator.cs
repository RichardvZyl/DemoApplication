namespace DemoApplication.Validator;

public sealed class AddMakerCheckerModelValidator : NewMakerCheckerModelValidator
{
    public AddMakerCheckerModelValidator() 
        => RuleForMotivation();
           //RuleForFiles();

}
