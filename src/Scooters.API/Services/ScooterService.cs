using AutoMapper;
using FluentValidation;
using Scooters.Api.Domain;
using Scooters.Api.Models;
using Scooters.Data;
using Scooters.Data.Entities;

namespace Scooters.Api.Services;

public interface IScootersService
{
    public Task<Response<ScooterDB>> CreateAsync(ScooterCreate request);
    public Task<Response<ScooterDB>> UpdateAsync(ScooterUpdate request);
    public Task<Response<IEnumerable<ScooterDB>>> GetAsync(ScooterFilter request);
    public Task<Response<IEnumerable<ScooterLog2024>>> GetTop5LatestMotosLogsAsync();
}

public class ScooterService : IScootersService
{
    private readonly IScootersRepository _scooters;
    private readonly ILogger<ScooterService> _logger;
    private readonly IValidator<ScooterCreate> _validatorCreate;
    private readonly IValidator<ScooterUpdate> _validatorUpdate;
    private readonly IMapper _mapper;

    public ScooterService(IScootersRepository scooters,
        ILogger<ScooterService> logger,
        IValidator<ScooterCreate> validatorCreate, 
        IValidator<ScooterUpdate> validatorUpdate,
        IMapper mapper)
    {
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
        _logger = logger;
        _scooters = scooters;
        _mapper = mapper;
    }

    public async Task<Response<ScooterDB>> CreateAsync(ScooterCreate request)
    {
        try
        {
            var validation = _validatorCreate.Validate(request);

            if (!validation.IsValid)
            {
                return Response<ScooterDB>.CreateFailure("Os dados da requisição estão incorretos");
            }

            var rabbitMqConfig = new RabbitMqConfig();

            IEventPublisher eventPublisher = new ScooterPublisher(rabbitMqConfig);

            var scooter = new Scooter
            (
                request.Year,
                request.Model,
                request.LicencePlate,
                eventPublisher
            );

            var scooterDB = _mapper.Map<ScooterDB>(scooter);

            await _scooters.CreateAsync(scooterDB);

            scooter.Publish();

            return Response<ScooterDB>.CreateSuccess(scooterDB);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting scooters.");
            throw;
        }
    }

    public async Task<Response<ScooterDB>> UpdateAsync(ScooterUpdate request)
    {
        try
        {
            var validation = _validatorUpdate.Validate(request);

            if (!validation.IsValid)
            {
                return Response<ScooterDB>
                    .CreateFailure("Os dados da requisição estão incorretos");
            }

            var scooter = new ScooterDB
            {
                Year = request.Year,
                Model = request.Model,
                LicencePlate = request.LicencePlate
            };

            await _scooters.UpdateAsync(scooter);

            return Response<ScooterDB>.CreateSuccess(scooter);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting scooters.");
            throw;
        }
    }

    public async Task<Response<IEnumerable<ScooterDB>>> GetAsync(ScooterFilter request)
    {
        try
        {
            var response = await _scooters.GetScootersAsync(request.LicencePlate);
            return Response<IEnumerable<ScooterDB>>.CreateSuccess(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting scooters.");
            return Response<IEnumerable<ScooterDB>>.CreateFailure(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<ScooterLog2024>>> GetTop5LatestMotosLogsAsync()
    {
        try
        {
            var response = await _scooters.GetScooters2024Async();
            return Response<IEnumerable<ScooterLog2024>>.CreateSuccess(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting scooters.");
            return Response<IEnumerable<ScooterLog2024>>.CreateFailure(ex.Message);
        }
    }
}
