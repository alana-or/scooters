using Microsoft.Extensions.Logging;
using Motos.Data;

namespace Motos.Application;

public class SelectMoto
{
    private readonly IMotosRepository motoRepository;
    private readonly ILogger<SelectMoto> logger;

    public SelectMoto(IMotosRepository motoRepository, ILogger<SelectMoto> logger)
    {
        this.motoRepository = motoRepository;
        this.logger = logger;
    }

    public async Task<MotoResponse<IEnumerable<Moto>>> Handle(MotoFilterRequest request)
    {
        try
        {
            var response = await motoRepository.GetMotoAsync(request.Placa);
            return MotoResponse<IEnumerable<Moto>>.CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            return MotoResponse<IEnumerable<Moto>>.CreateFailureResponse(ex.Message);
        }
    }
}