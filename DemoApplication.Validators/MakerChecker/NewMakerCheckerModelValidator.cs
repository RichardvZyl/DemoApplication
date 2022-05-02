using Abstractions.Validator;
using DemoApplication.Models;
using FluentValidation;

namespace DemoApplication.Validator;

public abstract class NewMakerCheckerModelValidator : Validator<NewMakerCheckerModel>
{
    public void RuleForMotivation()
        => RuleFor(x => x.Motivation)
            .NotEmpty()
            .MaximumLength(500);

    public void RuleForFiles()
        => RuleFor(x => x.Files)
            .NotEmpty();

    public void RuleForAction()
        => RuleFor(x => x.Action)
            .NotEmpty();
            //.MaximumLength(50);

    public void RuleForModel()
        => RuleFor(x => x.Model)
            .NotEmpty();
}
