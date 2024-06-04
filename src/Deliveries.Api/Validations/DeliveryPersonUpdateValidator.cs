using FluentValidation;
using Deliveries.Api.Models;

namespace Deliveries.Api.Validations;

public class DeliveryPersonUpdateValidator : AbstractValidator<DeliveryPersonUpdateModel>
{
    public DeliveryPersonUpdateValidator()
    {
        RuleFor(model => model.Id)
            .NotEmpty()
            .WithMessage("Id is missing");

        RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Name is missing");

        RuleFor(model => model.Photo)
            .NotEmpty()
            .WithMessage("Photo is missing");
    }
}