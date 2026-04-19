namespace Synapse.Application.Interfaces;

public interface IMessageBus
{
    Task PublishNoteCreatedAsync(Guid noteId, string content);
}