namespace DemoApplication.Validator;

/// <summary> Add Document Validator </summary>
public sealed class AddDocumentModelValidator : DocumentModelValidator
{
    public AddDocumentModelValidator()
    {
        //RuleForId(); //auto generated
        //RuleForRelatedId(); //not used
        RuleForName();
        //RuleForDocumentSubject(); //not used
        RuleForContents();
        //RuleForDateTimeOffsetLoaded(); //auto generated
    }
}
