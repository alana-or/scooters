﻿using FluentValidation;
using Scooters.Api.Models;

namespace Scooters.Api.Validations;

public class ScooterUpdateValidator : AbstractValidator<ScooterUpdate>
{
    public ScooterUpdateValidator()
    {
        RuleFor(model => model.Id)
            .NotEmpty()
            .WithMessage("Id is missing");

        RuleFor(model => model.LicencePlate)
            .NotEmpty()
            .WithMessage("LicencePlate is missing");
    }
}