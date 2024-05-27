using FluentValidation;
using Scooters.Api.Models;

namespace Scooters.Api.Validations;

public class ScooterCreateValidator : AbstractValidator<ScooterCreate>
{
    public ScooterCreateValidator()
    {
        RuleFor(model => model.Year)
            .NotEmpty()
            .WithMessage("Deve existir Year");

        RuleFor(model => model.LicencePlate)
            .NotEmpty()
            .WithMessage("Deve existir LicencePlate");

        RuleFor(model => model.Model)
            .NotEmpty()
            .WithMessage("Deve existir Model");
    }
}