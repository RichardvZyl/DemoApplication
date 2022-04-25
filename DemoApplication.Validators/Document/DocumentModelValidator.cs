using Abstractions.Validator;
using DemoApplication.Models;
using FluentValidation;

namespace DemoApplication.Validator;

/// <summary> Document class Validator </summary>
public class DocumentModelValidator : Validator<DocumentModel>
{
    public void RuleForId()
        => RuleFor(x => x.Id)
            .NotEmpty();

    //public void RuleForRelatedId() 
    //    => RuleFor(x => x.RelatedId)
    //        .NotEmpty();

    public void RuleForName()
        => RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

    //public void RuleForDocumentSubject() 
    //    => RuleFor(x => x.DocumentSubject)
    //        .NotEmpty()
    //        .MaximumLength(100);

    public void RuleForContents()
        => RuleFor(x => x.Contents)
            .NotEmpty();

    //public void RuleForDateTimeOffsetLoaded()
    //    => RuleFor(x => x.DateTimeOffsetLoaded)
    //        .GreaterThan(DateTimeOffset.MinValue)
    //        .LessThan(DateTimeOffset.MaxValue);

}
