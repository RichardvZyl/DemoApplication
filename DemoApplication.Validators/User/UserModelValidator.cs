using FluentValidation;
using Abstractions.Validator;
using DemoApplication.Models;

namespace DemoApplication.Validator;

/// <summary>  user model validator </summary>
public abstract class UserModelValidator : Validator<UserModel>
{
    public void RuleForAuth() 
        => RuleFor(x => x.Auth)
            .SetValidator(new AuthModelValidator());

    public void RuleForEmail() 
        => RuleFor(x => x.Email)
            .SetValidator(new EmailValidation());

    public void RuleForId() 
        => RuleFor(x => x.Id)
            .NotEmpty();

    public void RuleForName() 
        => RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

    public void RuleForSurname() 
        => RuleFor(x => x.Surname)
            .NotEmpty()
            .MaximumLength(50);
}
