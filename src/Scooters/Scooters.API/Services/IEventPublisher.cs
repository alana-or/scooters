namespace Scooters.Api.Services;

public interface IEventPublisher
{
    void Publish<T>(T eventMessage) where T : class;
}
