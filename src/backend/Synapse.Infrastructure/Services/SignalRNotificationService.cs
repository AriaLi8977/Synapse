using Synapse.Application.Interfaces;
using Synapse.Infrastructure.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace Synapse.Infrastructure.Services;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NoteHub> _hubContext;

    public SignalRNotificationService(IHubContext<NoteHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyNoteCompleted(Guid noteId, string summary, Guid userId)
    {
        var groupName = $"note-{noteId}";
        await _hubContext.Clients.Group(groupName)
                            .SendAsync("NoteCompleted", new
                            {
                                NoteId = noteId,
                                Summary = summary
                            });
    }

    public async Task NotifyNoteProcessing(Guid noteId)
    {
        var groupName = $"note-{noteId}";
        await _hubContext.Clients.Group(groupName).SendAsync("NoteProcessing", new
        {
            NoteId = noteId
        });
    }
}