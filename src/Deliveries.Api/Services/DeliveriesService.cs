using AutoMapper;
using FluentValidation;
using Deliveries.Api.Domain;
using Deliveries.Api.Models;
using Deliveries.Data;
using Deliveries.Data.Entities;

namespace Deliveries.Api.Services;

public interface IDeliveriesService
{
    public Task<Response<DeliveryPersonDb>> CreateAsync(DeliveryPersonCreate request);
    public Task<Response<DeliveryPersonDb>> UpdateAsync(DeliveryPersonUpdate request);
    public Task<Response<IEnumerable<DeliveryPersonDb>>> GetAsync();
}

public class DeliveriesService : IDeliveriesService
{
    private readonly IDeliveriesRepository _deliveryPersons;
    private readonly Serilog.ILogger _logger;
    private readonly IValidator<DeliveryPersonCreate> _validatorCreate;
    private readonly IValidator<DeliveryPersonUpdate> _validatorUpdate;
    private readonly IMapper _mapper;

    public DeliveriesService(IDeliveriesRepository deliveryPersons,
        IValidator<DeliveryPersonCreate> validatorCreate, 
        IValidator<DeliveryPersonUpdate> validatorUpdate,
        IMapper mapper)
    {
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
        _deliveryPersons = deliveryPersons;
        _mapper = mapper;
    }

    public async Task<Response<DeliveryPersonDb>> CreateAsync(DeliveryPersonCreate request)
    {
        try
        {
            var validation = _validatorCreate.Validate(request);

            if (!validation.IsValid)
            {
                return Response<DeliveryPersonDb>.CreateFailure("Os dados da requisição estão incorretos");
            }


            var deliveryPerson = new DeliveryPerson
            (
                request.Name,
                request.Photo
            );

            var deliveryPersonDB = _mapper.Map<DeliveryPersonDb>(deliveryPerson);

            await _deliveryPersons.CreateAsync(deliveryPersonDB);

            return Response<DeliveryPersonDb>.CreateSuccess(deliveryPersonDB);

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting deliveryPersons.");
            throw;
        }
    }

    public async Task<Response<DeliveryPersonDb>> UpdateAsync(DeliveryPersonUpdate request)
    {
        try
        {
            var validation = _validatorUpdate.Validate(request);

            if (!validation.IsValid)
            {
                return Response<DeliveryPersonDb>
                    .CreateFailure("Os dados da requisição estão incorretos");
            }

            var deliveryPerson = new DeliveryPerson
            (
                request.Name,
                request.Photo
            );

            var deliveryPersonDB = _mapper.Map<DeliveryPersonDb>(deliveryPerson);

            await _deliveryPersons.UpdateAsync(deliveryPersonDB);

            return Response<DeliveryPersonDb>.CreateSuccess(deliveryPersonDB);

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting deliveryPersons.");
            throw;
        }
    }

    public async Task<Response<IEnumerable<DeliveryPersonDb>>> GetAsync()
    {
        try
        {
            var response = await _deliveryPersons.GetDeliveryPeopleAsync();
            return Response<IEnumerable<DeliveryPersonDb>>.CreateSuccess(response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting deliveryPersons.");
            return Response<IEnumerable<DeliveryPersonDb>>.CreateFailure(ex.Message);
        }
    }
}
