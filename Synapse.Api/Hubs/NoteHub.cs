using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class NoteHub : Hubs
{
    public async Task JoinNoteGroup(string noteId)
    {
        var groupName = $"note-{noteId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveNoteGroup(string noteId)
    {
        var groupName = $"note-{noteId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}