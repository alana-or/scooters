using FluentValidation;
using Deliveries.Api.Models;

namespace Deliveries.Api.Validations;

public class DeliveryPersonValidator : AbstractValidator<DeliveryPersonModel>
{
    public DeliveryPersonValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Name is missing");

        RuleFor(model => model.Photo)
            .NotEmpty()
            .WithMessage("Photo is missing");
    }
}