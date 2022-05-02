using Abstractions.Regex;
using Abstractions.Validator;
using FluentValidation;

namespace DemoApplication.Validator;

public sealed class EmailValidation : Validator<string>
{
    public EmailValidation()
        => RuleFor(x => x)
            .Matches(Regexes.Email)
            .WithMessage("Valid email address required.")
            .MaximumLength(250)
            .WithMessage("Maximum length allowed for an email address is 250 characters.");
}

public sealed class MobileNumberValidation : Validator<string>
{
    public MobileNumberValidation()
        => RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Mobile Number Must Not be Empty.")
            .MinimumLength(10)
            .WithMessage("Mobile Number should be atleast 10 characters.")
            .Matches(Regexes.@MobileNumber)
            .WithMessage("Valid mobile number required.")
            .Must(x => x.ValidatePhoneNumber() == true)
            .WithMessage("Valid mobile number required.");
}

