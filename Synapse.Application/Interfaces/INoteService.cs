using Synapse.Application.DTOs;
using Synapse.Domain.Entities;

namespace Synapse.Application.Interfaces;

public interface INoteService
{
    Task<List<Note>> GetAllAsync(Guid userId);
    Task<Note> CreateAsync(CreateNoteDto dto, Guid userId);
}
