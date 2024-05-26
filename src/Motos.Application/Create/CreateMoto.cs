using FluentValidation;
using Microsoft.Extensions.Logging;
using Motos.Data;
using Motos.Data.Entities;
using Newtonsoft.Json;

namespace Motos.Application;

public class CreateMotoUseCase(IMotosRepository motoRepository,
    IValidator<CreateMotoRequest> validator,
    ILogger<CreateMotoUseCase> logger)
{
    private readonly IMotosRepository motoRepository = motoRepository;
    private readonly ILogger<CreateMotoUseCase> logger = logger;
    private readonly IValidator<CreateMotoRequest> validator = validator;

    public async Task<MotoResponse<Moto>> Handle(CreateMotoRequest request)
    {
        try
        {

            var validation = validator.Validate(request);

            if (!validation.IsValid)
            {
                return MotoResponse<Moto>.CreateFailureResponse("Os dados da requisição estão incorretos");
            }

            var moto = new Moto { 
                Ano = request.Ano, 
                Modelo = request.Modelo,
                Placa = request.Placa
            };

            await motoRepository.CreateMoto(moto);

            Publish(moto);

            return MotoResponse<Moto>.CreateSuccessResponse(moto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
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