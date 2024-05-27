using AutoMapper;
using FluentValidation;
using Motos.API.Domain;
using Motos.API.Models;
using Motos.Data;
using Motos.Data.Entities;
using Newtonsoft.Json;

namespace Motos.API.Services;

public interface IMotoService
{
    public Task<MotoResponse<MotoDB>> Create(CreateMotoRequest request);
    public Task<MotoResponse<MotoDB>> Update(UpdateMotoRequest request);
    public Task<MotoResponse<IEnumerable<MotoDB>>> Get(MotoFilterRequest request);
    public Task<MotoResponse<IEnumerable<MotosLog2024>>> GetTop5LatestMotosLogsAsync();
}

public class MotoService : IMotoService
{
    private readonly IMotosRepository motoRepository;
    private readonly ILogger<MotoService> logger;
    private readonly IValidator<CreateMotoRequest> validatorCreate;
    private readonly IValidator<UpdateMotoRequest> validatorUpdate;
    private readonly IMapper mapper;

    public MotoService(IMotosRepository motoRepository,
        ILogger<MotoService> logger,
        IValidator<CreateMotoRequest> validatorCreate, 
        IValidator<UpdateMotoRequest> validatorUpdate,
        IMapper mapper)
    {
        this.validatorCreate = validatorCreate;
        this.validatorUpdate = validatorUpdate;
        this.logger = logger;
        this.motoRepository = motoRepository;
        this.mapper = mapper;
    }

    public async Task<MotoResponse<MotoDB>> Create(CreateMotoRequest request)
    {
        try
        {
            var validation = validatorCreate.Validate(request);

            if (!validation.IsValid)
            {
                return MotoResponse<MotoDB>.CreateFailureResponse("Os dados da requisição estão incorretos");
            }

            var moto = new Moto
            {
                Ano = request.Ano,
                Modelo = request.Modelo,
                Placa = request.Placa
            };

            var motoDB = mapper.Map<MotoDB>(moto);

            await motoRepository.Create(motoDB);

            moto.Publish();

            return MotoResponse<MotoDB>.CreateSuccessResponse(motoDB);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task<MotoResponse<MotoDB>> Update(UpdateMotoRequest request)
    {
        try
        {
            var validation = validatorUpdate.Validate(request);

            if (!validation.IsValid)
            {
                return MotoResponse<MotoDB>
                    .CreateFailureResponse("Os dados da requisição estão incorretos");
            }

            var moto = new MotoDB
            {
                Ano = request.Ano,
                Modelo = request.Modelo,
                Placa = request.Placa
            };

            await motoRepository.Update(moto);

            return MotoResponse<MotoDB>.CreateSuccessResponse(moto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            throw;
        }
    }

    public async Task<MotoResponse<IEnumerable<MotoDB>>> Get(MotoFilterRequest request)
    {
        try
        {
            var response = await motoRepository.GetMotoAsync(request.Placa);
            return MotoResponse<IEnumerable<MotoDB>>.CreateSuccessResponse(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while getting motos.");
            return MotoResponse<IEnumerable<MotoDB>>.CreateFailureResponse(ex.Message);
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
}
