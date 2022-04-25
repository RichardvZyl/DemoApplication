using Abstractions.Validator;
using DemoApplication.Models;
using FluentValidation;

namespace DemoApplication.Validator;

public abstract class MakerCheckerModelValidator : Validator<MakerCheckerModel>
{
    public void RuleForId()
        => RuleFor(x => x.Id)
            .NotEmpty();

    public void RuleForAction()
        => RuleFor(x => x.Action)
            .NotEmpty();
            //.MaximumLength(50);

    public void RuleForMakerUser()
        => RuleFor(x => x.MakerUser)
            .NotEmpty();
            //.MaximumLength(200);

    public void RuleForCheckerUser()
        => RuleFor(x => x.CheckerUser)
            .NotEmpty();
            //.MaximumLength(200);

    public void RuleForAccepted()
        => RuleFor(x => x.Accepted)
            .NotEmpty();

    public void RuleForMakerDate()
        => RuleFor(x => x.MakerDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTimeOffset.MinValue)
            .LessThanOrEqualTo(DateTimeOffset.MaxValue);

    public void RuleForCheckerDate()
        => RuleFor(x => x.CheckerDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTimeOffset.MinValue)
            .LessThanOrEqualTo(DateTimeOffset.MaxValue);

    public void RuleForMotivation()
        => RuleFor(x => x.Motivation)
            .NotEmpty()
            .MaximumLength(500);

    public void RuleForFiles()
        => RuleFor(x => x.Files)
            .NotEmpty();

    public void RuleForModel()
        => RuleFor(x => x.Model)
            .NotEmpty();
}
