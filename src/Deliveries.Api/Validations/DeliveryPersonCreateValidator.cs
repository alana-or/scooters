using Deliveries.Api.Models;
using FluentValidation;

namespace Deliveries.Api.Validations;

public class DeliveryPersonCreateValidator : AbstractValidator<DeliveryPersonCreateModel>
{
    public DeliveryPersonCreateValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .WithMessage("Name is missing");

        RuleFor(model => model.CNHImage)
            .NotEmpty()
            .WithMessage("CNHImage is missing");

        RuleFor(model => model.CNHType)
            .NotEmpty().WithMessage("CNHType is required.")
            .Must(BeAorB).WithMessage("CNHType must be A or B.");
    }

    private bool BeAorB(char cnhType)
    {
        return cnhType == 'A' || cnhType == 'B';
    }
}