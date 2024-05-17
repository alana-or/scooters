using FluentValidation;
using Microsoft.Extensions.Logging;
using Motos.Data;
using Motto.Entities;

public class UpdateMotoUseCase
{
    private readonly IMotosRepository motoRepository;
    private readonly IValidator<UpdateMotoRequest> validator;
    private readonly ILogger<UpdateMotoUseCase> logger;

    public UpdateMotoUseCase(IMotosRepository motoRepository, 
        IValidator<UpdateMotoRequest> validator, 
        ILogger<UpdateMotoUseCase> logger)
    {
        this.motoRepository = motoRepository;
        this.logger = logger;
        this.validator = validator;
    }

    public MotoResponse<Moto> Handle(UpdateMotoRequest request)
    {
        try
        {

            var validation = validator.Validate(request);

            if (!validation.IsValid)
            {
                return MotoResponse<Moto>
                    .CreateFailureResponse("Os dados da requisição estão incorretos");
            }

            var moto = new Moto
            {
                ano = request.Ano,
                modelo = request.Modelo,
                placa = request.Placa
            };

            motoRepository.UpdateMoto(moto);

            return MotoResponse<Moto>.CreateSuccessResponse(moto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }
}