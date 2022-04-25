using Abstractions.Validator;
using DemoApplication.Models;
using FluentValidation;

namespace DemoApplication.Validator;

public sealed class SignInModelValidator : Validator<SignInModel>
{
    public SignInModelValidator()
    {
        _ = RuleFor(x => x.Login).NotEmpty();
        _ = RuleFor(x => x.Password).NotEmpty();
    }
}
