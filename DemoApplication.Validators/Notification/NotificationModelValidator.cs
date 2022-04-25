using Abstractions.Validator;
using DemoApplication.Models;
using FluentValidation;

namespace DemoApplication.Validator;

public abstract class NotificationModelValidator : Validator<NotificationModel>
{
    public void RuleForId()
        => RuleFor(x => x.Id)
            .NotEmpty();
    public void RuleForOriginator()
        => RuleFor(x => x.Originator)
            .NotEmpty();
            //.MaximumLength(50);       

    public void RuleForSeverity()
        => RuleFor(x => x.Severity)
            .IsInEnum();

    public void RuleForDescription()
        => RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(100);

    public void RuleForDate()
        => RuleFor(x => x.Date)
            .GreaterThanOrEqualTo(DateTimeOffset.MinValue)
            .LessThanOrEqualTo(DateTimeOffset.MaxValue);

    public void RuleForSeenBy()
        => RuleFor(x => x.SeenBy)
            .NotEmpty();

    public void RuleForSeenAt()
        => RuleFor(x => x.SeenAt)
            .NotEmpty();

    public void RuleForForRole()
        => RuleFor(x => x.ForRole)
            .IsInEnum();

    public void RuleForEntity()
        => RuleFor(x => x.Entity)
            .IsInEnum();

    public void RuleForRelatedId()
        => RuleFor(x => x.RelatedId)
            .NotEmpty();
}
