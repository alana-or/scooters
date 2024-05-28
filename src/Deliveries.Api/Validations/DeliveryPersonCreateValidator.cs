using FluentValidation;
using Deliveries.Api.Models;

namespace Deliveries.Api.Validations;

public class DeliveryPersonCreateValidator : AbstractValidator<DeliveryPersonCreate>
{
    public DeliveryPersonCreateValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Deve existir Name");

        RuleFor(model => model.Photo)
            .NotEmpty()
            .WithMessage("Deve existir Photo");
    }
}