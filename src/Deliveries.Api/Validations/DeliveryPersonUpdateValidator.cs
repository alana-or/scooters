using FluentValidation;
using Deliveries.Api.Models;

namespace Deliveries.Api.Validations;

public class DeliveryPersonUpdateValidator : AbstractValidator<DeliveryPersonUpdate>
{
    public DeliveryPersonUpdateValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Deve existir LicencePlate");

        RuleFor(model => model.Photo)
            .NotEmpty()
            .WithMessage("Deve existir Model");
    }
}