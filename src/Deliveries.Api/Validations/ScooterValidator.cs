using Deliveries.Application.Models;
using FluentValidation;

namespace Deliveries.Api.Validations;

public class ScooterValidator : AbstractValidator<ScooterModel>
{
    public ScooterValidator()
    {
        RuleFor(model => model.Id)
            .NotEmpty()
            .WithMessage("Id is missing");

        RuleFor(model => model.Model)
            .NotEmpty()
            .WithMessage("Name is missing");

        RuleFor(model => model.Year)
            .NotEmpty()
            .WithMessage("Year is missing");

        RuleFor(model => model.LicencePlate)
            .NotEmpty()
            .WithMessage("LicencePlate is missing");
    }
}