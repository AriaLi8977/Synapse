using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Synapse.Infrastructure.Realtime;

[Authorize]
public class NoteHub : Hub
{

    private readonly INoteRepository _noteRep;
    public NoteHub(INoteRepository noteRep)
    {
        _noteRep = noteRep;
    }
    public async Task JoinNoteGroup(string noteId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new HubException("Unauthorized");
        var note = await _noteRep.GetByIdAsync(Guid.Parse(noteId));
        if(note == null)
            throw new HubException("Note not found");

        if (note.UserId.ToString() != userId)
            throw new HubException("Access denied");

        var groupName = $"note-{noteId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveNoteGroup(string noteId)
    {
        var groupName = $"note-{noteId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}