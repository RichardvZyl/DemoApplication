using FluentValidation;
using Abstractions.Validator;
using Abstractions.Regex;
using DemoApplication.Models;

namespace DemoApplication.Validator;

public sealed class AuthModelValidator : Validator<AuthModel>
{
    public AuthModelValidator()
    {
        _ = RuleFor(x => x.Login).NotEmpty().MaximumLength(100);
        _ = RuleFor(x => x.Password).NotEmpty().MaximumLength(500).Matches(Regexes.Password); 
        _ = RuleFor(x => x.Role).IsInEnum();
    }
}
