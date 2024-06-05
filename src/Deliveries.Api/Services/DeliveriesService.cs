using AutoMapper;
using Deliveries.Api.Domain;
using Deliveries.Api.Models;
using Deliveries.Data;
using Deliveries.Data.Entities;
using FluentValidation;

namespace Deliveries.Api.Services;

public interface IDeliveriesService
{
    public Task<Response<DeliveryPersonModel>> CreatePersonAsync(DeliveryPersonCreateModel request);
    public Task<Response<DeliveryPersonModel>> UpdatePersonAsync(DeliveryPersonUpdateModel request);
    public Task<Response<IEnumerable<RentalModel>>> GetPersonRentalsAsync(Guid idPerson);
    public Task<Response<RentalModel>> CreateRentalAsync(DeliveryPersonRentalCreateModel request);
    public Task<Response<IEnumerable<ScooterModel>>> GetScootersAsync();
}

public class DeliveriesService : IDeliveriesService
{
    private readonly IDeliveryPersonRepository _deliveryPeople;
    private readonly IDeliveryPersonRentalsRepository _deliveryPersonRentals;
    private readonly ILogger<DeliveriesService> _logger;
    private readonly IValidator<DeliveryPersonCreateModel> _validatorCreate;
    private readonly IValidator<DeliveryPersonUpdateModel> _validatorUpdate;
    private readonly IValidator<DeliveryPersonRentalCreateModel> _validatorRental;
    private readonly IMapper _mapper;

    public DeliveriesService(IDeliveryPersonRepository deliveryPeople,
        IDeliveryPersonRentalsRepository deliveryPersonRentals,
        IValidator<DeliveryPersonCreateModel> validatorCreate, 
        IValidator<DeliveryPersonUpdateModel> validatorUpdate,
        ILogger<DeliveriesService> logger,
        IMapper mapper,
        IValidator<DeliveryPersonRentalCreateModel> validatorRental)
    {
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
        _deliveryPeople = deliveryPeople;
        _deliveryPersonRentals = deliveryPersonRentals;
        _logger = logger;
        _mapper = mapper;
        _validatorRental = validatorRental;
    }

    public async Task<Response<DeliveryPersonModel>> CreatePersonAsync(DeliveryPersonCreateModel request)
    {
        try
        {
            var validation = _validatorCreate.Validate(request);

            if (!validation.IsValid)
            {
                return Response<DeliveryPersonModel>.CreateFailure("Request has invalid data.");
            }

            var deliveryPerson = new DeliveryPerson
            (
                request.Name,
                request.CNHImage,
                request.CNPJ,
                request.CNH,
                request.CNHType,
                request.Birth
            );

            var deliveryPersonDb = _mapper.Map<DeliveryPersonDb>(deliveryPerson);

            await _deliveryPeople.CreateAsync(deliveryPersonDb);

            var deliveryPersonResponse = _mapper.Map<DeliveryPersonModel>(deliveryPersonDb);

            return Response<DeliveryPersonModel>.CreateSuccess(deliveryPersonResponse);
        }
        catch (Exception ex)
        {
            var message = "An error occurred while creating a person.";
            _logger.LogError(ex, message);
            return Response<DeliveryPersonModel>.CreateFailure(message);
        }
    }

    public async Task<Response<DeliveryPersonModel>> UpdatePersonAsync(DeliveryPersonUpdateModel request)
    {
        try
        {
            var validation = _validatorUpdate.Validate(request);

            if (!validation.IsValid)
            {
                return Response<DeliveryPersonModel>.CreateFailure("Request has invalid data.");
            }

            var deliveryPesonDb = await _deliveryPeople.GetDeliveryPersonAsync(request.Id);

            var deliveryPerson = _mapper.Map<DeliveryPerson>(deliveryPesonDb);

            deliveryPerson.UpdateCNHImage(request.CNHImage);

            var deliveryPersonDB = _mapper.Map<DeliveryPersonDb>(deliveryPerson);

            await _deliveryPeople.UpdateAsync(deliveryPersonDB);

            var deliveryPersonResponse = _mapper.Map<DeliveryPersonModel>(deliveryPersonDB);

            return Response<DeliveryPersonModel>.CreateSuccess(deliveryPersonResponse);

        }
        catch (Exception ex)
        {
            var message = "An error occurred while updating a person.";
            _logger.LogError(ex, message);
            return Response<DeliveryPersonModel>.CreateFailure(message);
        }
    }

    public async Task<Response<IEnumerable<ScooterModel>>> GetScootersAsync()
    {
        try
        {
            var response = new List<ScooterModel>();
            return Response<IEnumerable<ScooterModel>>.CreateSuccess(response);
        }
        catch (Exception ex)
        {
            var message = "An error occurred while getting scooters.";
            _logger.LogError(ex, message);
            return Response<IEnumerable<ScooterModel>>.CreateFailure(message);
        }
    }

    public async Task<Response<RentalModel>> CreateRentalAsync(DeliveryPersonRentalCreateModel request)
    {
        try
        {
            var validation = _validatorRental.Validate(request);

            if (!validation.IsValid)
            {
                return Response<RentalModel>.CreateFailure("Request has invalid data.");
            }

            var deliveryPesonDb = await _deliveryPeople.GetDeliveryPersonAsync(request.DeliveryPersonId);

            var deliveryPerson = _mapper.Map<DeliveryPerson>(deliveryPesonDb);

            //validar dados para fazer aluguel
            var deliveryRental = new DeliveryPersonRental
            (
                request.Scooter.Id,
                request.Scooter.Year,
                request.Scooter.Model,
                request.Scooter.LicencePlate,
                deliveryPerson
            );

            var deliveryRentalDb = _mapper.Map<DeliveryPersonRentalDb>(deliveryRental);

            await _deliveryPersonRentals.CreateAsync(deliveryRentalDb);

            var deliveryRentalResponse = _mapper.Map<RentalModel>(deliveryRentalDb);

            return Response<RentalModel>.CreateSuccess(deliveryRentalResponse);
        }
        catch (Exception ex)
        {
            var message = "An error occurred while creating rental.";
            _logger.LogError(ex, message);
            return Response<RentalModel>.CreateFailure(message);
        }
    }

    public async Task<Response<IEnumerable<RentalModel>>> GetPersonRentalsAsync(Guid idPerson)
    {
        try
        {
            var rentals = await _deliveryPersonRentals.GetDeliveryPersonRentalsAsync(idPerson);

            var deliveryRentalResponse = _mapper.Map<IEnumerable<RentalModel>>(rentals);

            return Response<IEnumerable<RentalModel>>.CreateSuccess(deliveryRentalResponse);
        }
        catch (Exception ex)
        {
            var message = "An error occurred while getting rentals.";
            _logger.LogError(ex, message);
            return Response<IEnumerable<RentalModel>>.CreateFailure(message);
        }
    }
}
