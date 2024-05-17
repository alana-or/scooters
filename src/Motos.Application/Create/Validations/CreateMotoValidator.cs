using FluentValidation;

namespace Motos.Application.Create.Validations;

public class CreateMotoValidator : AbstractValidator<CreateMotoRequest>
{
    public CreateMotoValidator()
    {
        RuleFor(model => model.Ano)
            .NotEmpty()
            .WithMessage("Deve existir Ano");

        RuleFor(model => model.Placa)
            .NotEmpty()
            .WithMessage("Deve existir Placa");

        RuleFor(model => model.Modelo)
            .NotEmpty()
            .WithMessage("Deve existir Modelo");
    }
}