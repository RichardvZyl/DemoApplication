using Abstractions.Validator;
using DemoApplication.Models;
using FluentValidation;

namespace DemoApplication.Validator;

public abstract class AuditTrailModelValidator : Validator<AuditTrailModel>
{
    public void RuleForId()
        => RuleFor(x => x.Id)
            .NotEmpty(); //auto generated

    public void RuleForDate()
        => RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTimeOffset.MaxValue)
            .GreaterThanOrEqualTo(DateTimeOffset.MinValue);

    public void RuleForUserId()
        => RuleFor(x => x.UserId)
            .NotEmpty();

    public void RuleForDisplayContext()
        => RuleFor(x => x.DisplayContext)
            .NotEmpty();

    public void RuleForModel()
        => RuleFor(x => x.Model)
            .NotEmpty();

    public void RuleForContents()
        => RuleFor(x => x.Contents)
            .NotEmpty();
}
