using FluentValidation;
using Microsoft.Extensions.Logging;
using Motos.Data;
using Motos.Data.Entities;

namespace Motos.Services;

public class UpdateMoto
{
    private readonly IMotosRepository motoRepository;
    private readonly IValidator<UpdateMotoRequest> validator;
    private readonly ILogger<UpdateMoto> logger;

    public UpdateMoto(IMotosRepository motoRepository, 
        IValidator<UpdateMotoRequest> validator, 
        ILogger<UpdateMoto> logger)
    {
        this.motoRepository = motoRepository;
        this.logger = logger;
        this.validator = validator;
    }

    public async Task<MotoResponse<Moto>> Handle(UpdateMotoRequest request)
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
                Ano = request.Ano,
                Modelo = request.Modelo,
                Placa = request.Placa
            };

            await motoRepository.UpdateMoto(moto);

            return MotoResponse<Moto>.CreateSuccessResponse(moto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }
}