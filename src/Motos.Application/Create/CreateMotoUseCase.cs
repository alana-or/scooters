using FluentValidation;
using Microsoft.Extensions.Logging;
using Motos.Data;
using Motto.Entities;

public class CreateMotoUseCase
{
    private readonly IMotosRepository motoRepository;
    private readonly ILogger<CreateMotoUseCase> logger;
    private readonly IValidator<CreateMotoRequest> validator;

    public CreateMotoUseCase(IMotosRepository motoRepository, 
        IValidator<CreateMotoRequest> validator,
        ILogger<CreateMotoUseCase> logger)
    {
        this.motoRepository = motoRepository;
        this.validator = validator;
        this.logger = logger;
    }

    public MotoResponse<Moto> Handle(CreateMotoRequest request)
    {
        try
        {

        var validation = validator.Validate(request);

        if (!validation.IsValid)
        {
            return MotoResponse<Moto>.CreateFailureResponse("Os dados da requisição estão incorretos");
        }

        var moto = new Moto { 
            Id = new Guid(), 
            Ano = request.Ano, 
            Modelo = request.Modelo,
            Placa = request.Placa
        };

        motoRepository.CreateMoto(moto);

        return MotoResponse<Moto>.CreateSuccessResponse(moto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }
}