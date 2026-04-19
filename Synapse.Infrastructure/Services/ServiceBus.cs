using Azure.Messaging.ServiceBus;
using System.Text.Json;
using Synapse.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Synapse.Infrastructure.Services;

public class ServiceBus : IMessageBus
{
    private readonly ServiceBusSender _sender;

    public ServiceBus(IConfiguration config)
    {
        var client = new ServiceBusClient(config["ServiceBus"]);
        _sender = client.CreateSender("NotesQueue");
    }

    public async Task PublishNoteCreatedAsync(Guid noteId, string content)
    {
        var message = new
        {
            NoteId = noteId,
            Content = content
        };

        var json = JsonSerializer.Serialize(message);

        await _sender.SendMessageAsync(new ServiceBusMessage(json));
    }
}