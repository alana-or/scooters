using Microsoft.Extensions.Logging;
using Motos.Data;
using Motto.Entities;

public class SelectMotoUseCase
{
    private readonly IMotosRepository motoRepository;
    private readonly ILogger<SelectMotoUseCase> logger;

    public SelectMotoUseCase(IMotosRepository motoRepository, ILogger<SelectMotoUseCase> logger)
    {
        this.motoRepository = motoRepository;
        this.logger = logger;
    }

    public async Task<MotoResponse<IEnumerable<Moto>>> Handle(MotoFilterRequest request)
    {
        try
        {
            var response = motoRepository.GetMotoAsync(request.Placa);
            return MotoResponse<IEnumerable<Moto>>.CreateSuccessResponse(response.Result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            return MotoResponse<IEnumerable<Moto>>.CreateFailureResponse(ex.Message);
        }
    }
}