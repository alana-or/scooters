using FluentValidation;
using Motos.API.Models;
using Motos.Data;
using Motos.Data.Entities;
using Newtonsoft.Json;

namespace Motos.API.Services;

public interface IMotoService
{
    public Task<MotoResponse<Moto>> Create(CreateMotoRequest request);
    public Task<MotoResponse<Moto>> Update(UpdateMotoRequest request);
    public Task<MotoResponse<IEnumerable<Moto>>> Get(MotoFilterRequest request);
    public Task<MotoResponse<IEnumerable<MotosLog2024>>> GetTop5LatestMotosLogsAsync();
}

public class MotoService : IMotoService
{
    private readonly IMotosRepository motoRepository;
    private readonly ILogger<MotoService> logger;
    private readonly IValidator<CreateMotoRequest> validatorCreate;
    private readonly IValidator<UpdateMotoRequest> validatorUpdate;

    public MotoService(IMotosRepository motoRepository,
        ILogger<MotoService> logger,
        IValidator<CreateMotoRequest> validatorCreate, 
        IValidator<UpdateMotoRequest> validatorUpdate)
    {
        this.validatorCreate = validatorCreate;
        this.validatorUpdate = validatorUpdate;
        this.logger = logger;
        this.motoRepository = motoRepository;
    }

    public async Task<MotoResponse<Moto>> Create(CreateMotoRequest request)
    {
        try
        {
            var validation = validatorCreate.Validate(request);

            if (!validation.IsValid)
            {
                return MotoResponse<Moto>.CreateFailureResponse("Os dados da requisição estão incorretos");
            }

            var moto = new Moto
            {
                Ano = request.Ano,
                Modelo = request.Modelo,
                Placa = request.Placa
            };

            await motoRepository.Create(moto);

            Publish(moto);

            return MotoResponse<Moto>.CreateSuccessResponse(moto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task<MotoResponse<Moto>> Update(UpdateMotoRequest request)
    {
        try
        {
            var validation = validatorUpdate.Validate(request);

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

            await motoRepository.Update(moto);

            return MotoResponse<Moto>.CreateSuccessResponse(moto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task<MotoResponse<IEnumerable<Moto>>> Get(MotoFilterRequest request)
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

    public async Task<MotoResponse<IEnumerable<MotosLog2024>>> GetTop5LatestMotosLogsAsync()
    {
        try
        {
            var response = await motoRepository.GetMoto2024Async();
            return MotoResponse<IEnumerable<MotosLog2024>>.CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            return MotoResponse<IEnumerable<MotosLog2024>>.CreateFailureResponse(ex.Message);
        }
    }

    private static void Publish(Moto moto)
    {
        var publisher = new MotosPublisher(hostname: "motos_rabbit",
                queueName: "motos_queue",
                username: "guest",
                password: "guest",
                port: 5672);

        string message = JsonConvert.SerializeObject(moto);
        publisher.PublishMessage(message);
        publisher.Dispose();
    }
}
