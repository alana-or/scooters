using FluentValidation;
using Microsoft.Extensions.Logging;
using Motos.Data;
using Motto.Entities;
using Newtonsoft.Json;
using System;

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

    private void Publish(Moto moto)
    {
        var hostname = "localhost";
        var queueName = "your_queue_name";
        var username = "guest";
        var password = "guest";

        // Publish a message
        var publisher = new RabbitMQPublisher(hostname, queueName, username, password);

        string message = JsonConvert.SerializeObject(moto);
        publisher.PublishMessage(message);
        publisher.Dispose();

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}