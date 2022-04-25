namespace DemoApplication.Validator;

/// <summary> Add Document Validator </summary>
public sealed class UpdateDocumentModelValidator : DocumentModelValidator
{
    public UpdateDocumentModelValidator()
    {
        RuleForId();
        //RuleForRelatedId(); //not used
        RuleForName();
        //RuleForDocumentSubject(); //not used
        RuleForContents();
        //RuleForDateTimeOffsetLoaded(); //auto generated
    }
}
