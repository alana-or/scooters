using FluentValidation;
using Scooters.Api.Models;

namespace Scooters.Api.Validations;

public class ScooterCreateValidator : AbstractValidator<ScooterCreate>
{
    public ScooterCreateValidator()
    {
        RuleFor(model => model.Year)
            .NotEmpty()
            .WithMessage("Year is missing");

        RuleFor(model => model.LicencePlate)
            .NotEmpty()
            .WithMessage("LicencePlate is missing");

        RuleFor(model => model.Model)
            .NotEmpty()
            .WithMessage("Model is missing");
    }
}