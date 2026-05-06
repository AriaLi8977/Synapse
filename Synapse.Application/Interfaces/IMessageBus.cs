namespace Synapse.Application.Interfaces;

public interface IMessageBus
{
    Task PublishAsync<T>(T message);
}