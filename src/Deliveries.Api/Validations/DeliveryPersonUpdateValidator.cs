﻿using FluentValidation;
using Deliveries.Api.Models;

namespace Deliveries.Api.Validations;

public class DeliveryPersonUpdateValidator : AbstractValidator<DeliveryPersonUpdateModel>
{
    public DeliveryPersonUpdateValidator()
    {
        RuleFor(model => model.Id)
            .NotEmpty()
            .WithMessage("Id is missing");

        RuleFor(model => model.CNHImage)
            .NotEmpty()
            .WithMessage("CNHImage is missing");
    }
}