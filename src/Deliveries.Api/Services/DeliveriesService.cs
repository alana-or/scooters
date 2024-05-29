using AutoMapper;
using Deliveries.Api.Domain;
using Deliveries.Api.Models;
using Deliveries.Data;
using Deliveries.Data.Entities;
using FluentValidation;

namespace Deliveries.Api.Services;

public interface IDeliveriesService
{
    public Task<Response<DeliveryPersonResponse>> CreateAsync(DeliveryPersonCreate request);
    public Task<Response<DeliveryPersonResponse>> UpdateAsync(DeliveryPersonUpdate request);
    public Task<Response<IEnumerable<Rental>>> GetRentalsAsync(Guid request);
    public Task<Response<Rental>> CreateRentalAsync(Rental request);
    public Task<Response<IEnumerable<Scooter>>> GetScootersAsync();
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

    public async Task<Response<DeliveryPersonResponse>> CreateAsync(DeliveryPersonCreate request)
    {
        try
        {
            var validation = _validatorCreate.Validate(request);

            if (!validation.IsValid)
            {
                return Response<DeliveryPersonResponse>.CreateFailure("Os dados da requisição estão incorretos");
            }


            var deliveryPerson = new DeliveryPerson
            (
                request.Name,
                request.Photo
            );

            var deliveryPersonDB = _mapper.Map<DeliveryPersonDb>(deliveryPerson);

            await _deliveryPersons.CreateAsync(deliveryPersonDB);

            var deliveryPersonResponse = _mapper.Map<DeliveryPersonResponse>(deliveryPersonDB);

            return Response<DeliveryPersonResponse>.CreateSuccess(deliveryPersonResponse);

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting deliveryPersons.");
            throw;
        }
    }

    public async Task<Response<DeliveryPersonResponse>> UpdateAsync(DeliveryPersonUpdate request)
    {
        try
        {
            var validation = _validatorUpdate.Validate(request);

            if (!validation.IsValid)
            {
                return Response<DeliveryPersonResponse>
                    .CreateFailure("Os dados da requisição estão incorretos");
            }

            var deliveryPerson = new Domain.DeliveryPerson
            (
                request.Name,
                request.Photo
            );

            var deliveryPersonDB = _mapper.Map<DeliveryPersonDb>(deliveryPerson);

            await _deliveryPersons.UpdateAsync(deliveryPersonDB);

            var deliveryPersonResponse = _mapper.Map<DeliveryPersonResponse>(deliveryPersonDB);

            return Response<DeliveryPersonResponse>.CreateSuccess(deliveryPersonResponse);

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting deliveryPersons.");
            throw;
        }
    }

    public async Task<Response<IEnumerable<Scooter>>> GetScootersAsync()
    {
        try
        {
            var response = new List<Scooter>();
            return Response<IEnumerable<Scooter>>.CreateSuccess(response);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting scooters.");
            return Response<IEnumerable<Scooter>>.CreateFailure(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<Rental>>> GetRentalsAsync(Guid request)
    {
        try
        {
            var response = await _deliveryPersons.GetRentals(request);
            var rentals = _mapper.Map<IEnumerable<Rental>>(response);

            return Response<IEnumerable<Rental>>.CreateSuccess(rentals);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting rentals.");
            return Response<IEnumerable<Rental>>.CreateFailure(ex.Message);
        }
    }

    public async Task<Response<Rental>> CreateRentalAsync(Rental request)
    {
        try
        {
            //var validation = _validatorCreate.Validate(request);

            //if (!validation.IsValid)
            //{
            //    return Response<DeliveryPerson>.CreateFailure("Os dados da requisição estão incorretos");
            //}

            var deliveryRental = new DeliveryPersonRentals
            (
                request.Scooter.Id,
                request.Scooter.Year,
                request.Scooter.Model,
                request.Scooter.LicencePlate,
                new DeliveryPerson(request.DeliveryPerson.Name, 
                    request.DeliveryPerson.Photo)
            );

            var deliveryRentalDb = _mapper.Map<DeliveryPersonRentalDb>(deliveryRental);

            await _deliveryPersons.CreateDeliveryRentalAsync(deliveryRentalDb);

            var deliveryRentalResponse = _mapper.Map<Rental>(deliveryRentalDb);

            return Response<Rental>.CreateSuccess(deliveryRentalResponse);

        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while getting deliveryPersons.");
            throw;
        }
    }
}
