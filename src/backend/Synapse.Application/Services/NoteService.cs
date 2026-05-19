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
        return await _noteRepository.GetAllAsync(userId);
    }

}