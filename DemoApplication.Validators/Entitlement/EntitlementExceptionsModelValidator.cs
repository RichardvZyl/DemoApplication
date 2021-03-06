using Abstractions.Validator;
using DemoApplication.Models;
using FluentValidation;

namespace DemoApplication.Validator;

public class EntitlementExceptionsModelValidator : Validator<EntitlementExceptionsModel>
{
    public void RuleForUserId()
        => RuleFor(x => x.UserId).NotEmpty();
}
