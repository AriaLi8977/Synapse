using Synapse.Application.DTOs;
using Synapse.Application.Interfaces;
using Synapse.Domain.Entities;

namespace Synapse.Application.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<List<Note>> GetAllAsync(Guid userId)
    {
        return await _noteRepository.GetAllAsync(userId);
    }

    public async Task<Note> CreateAsync(CreateNoteDto dto, Guid userId)
    {
        var note = new Note
        {
            Id = Guid.NewGuid(),
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow
        };
        await _noteRepository.AddAsync(note, userId);
        return note;
    }

}