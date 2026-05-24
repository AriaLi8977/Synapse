using Synapse.Application.DTOs;
using Synapse.Domain.Entities;

namespace Synapse.Application.Interfaces;

public interface INoteService
{
    Task<List<Note>> GetAllAsync(Guid userId);
    // Task<Note> CreateAsync(CreateNoteDto dto, Guid userId); //moving this to use case
    Task<Note?> GetByIdAsync(Guid id, Guid userId);
    Task UpdateAsync(Guid id, UpdateNoteDto dto, Guid userId);
    // Task DeleteAsync(Guid id, Guid userId);
}
