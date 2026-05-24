using Synapse.Application.DTOs;
using Synapse.Application.Interfaces;
using Synapse.Domain.Entities;


namespace Synapse.Application.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IMessageBus _messageBus;
    public NoteService(INoteRepository noteRepository, IMessageBus messageBus)
    {
        _noteRepository = noteRepository;
        _messageBus = messageBus;
    }

    public async Task<List<Note>> GetAllAsync(Guid userId)
    {
        var notes = await _noteRepository.GetAllAsync(userId);
        return notes.OrderByDescending(x => x.CreatedAt).ToList();
    }

    public async Task<Note?> GetByIdAsync(Guid id, Guid userId)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null || note.UserId != userId)
            return null;
        return note;
    }

    public async Task UpdateAsync(Guid id, UpdateNoteDto dto, Guid userId)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null || note.UserId != userId)
            throw new Exception("Note not found or access denied");
        note.Content = dto.Content;
        //note.Summary = dto.Summary; //going to create a re-generate summary use case instead of doing it here
        //note.Status = dto.Status; //same as above, will handle status changes in a separate use case
        await _noteRepository.UpdateAsync(note);
        await _messageBus.PublishAsync(new NoteMessageDto
        {
            NoteId = note.Id,
            Content = note.Content
        });
    }

    // public async Task DeleteAsync(Guid id, Guid userId)
    // {
    //     var note = await _noteRepository.GetByIdAsync(id);
    //     if (note == null || note.UserId != userId)
    //         throw new Exception("Note not found or access denied");
    //     await _noteRepository.DeleteAsync(id);
    // }
}