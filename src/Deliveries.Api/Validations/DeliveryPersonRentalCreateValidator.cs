using FluentValidation;
using Deliveries.Api.Models;

namespace Deliveries.Api.Validations;

public class DeliveryPersonRentalCreateValidator : AbstractValidator<DeliveryPersonRentalCreateModel>
{
    public DeliveryPersonRentalCreateValidator()
    {
        RuleFor(model => model.Scooter)
            .SetValidator(new ScooterValidator())
            .WithMessage("Scooter is missing");

        RuleFor(model => model.DeliveryPersonId)
            .NotEmpty()
            .WithMessage("DeliveryPersonId is missing");
    }
}