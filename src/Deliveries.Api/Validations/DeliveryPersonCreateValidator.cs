using FluentValidation;
using Deliveries.Api.Models;

namespace Deliveries.Api.Validations;

public class DeliveryPersonCreateValidator : AbstractValidator<DeliveryPersonCreateModel>
{
    public DeliveryPersonCreateValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Name is missing");

        RuleFor(model => model.Photo)
            .NotEmpty()
            .WithMessage("Photo is missing");
    }
}