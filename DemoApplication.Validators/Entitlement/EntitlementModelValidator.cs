using FluentValidation;
using Abstractions.Validator;
using DemoApplication.Models;

namespace DemoApplication.Validator;

public class EntitlementModelValidator : Validator<EntitlementModel>
{
    public void RuleForUserId()
        => RuleFor(x => x.UserId).NotEmpty();
}
