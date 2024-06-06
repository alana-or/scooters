using Scooters.Api.Services;

namespace Scooters.Api.Domain;

public class Scooter
{
    private readonly IEventPublisher _eventPublisher;

    public Guid Id { get; private set; }

    public int Year { get; private set; }

    public string Model { get; private set; }

    public string LicencePlate { get; private set; }

    public Scooter(int year, string model, string licencePlate, IEventPublisher eventPublisher, Guid guid = new Guid())
    {
        Id = guid;
        Year = year;
        Model = model;
        LicencePlate = licencePlate;
        _eventPublisher = eventPublisher;
    }

    public void Publish()
    {
        if(Year == 2024)
        {
            _eventPublisher.Publish(this); 
        }
    }

    internal Scooter SetId(Guid id)
    {
        this.Id = id;
        return this;
    }
}
