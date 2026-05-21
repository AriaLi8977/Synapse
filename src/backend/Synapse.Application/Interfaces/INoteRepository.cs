using Synapse.Domain.Entities;

public interface INoteRepository
{
    Task<List<Note>> GetAllAsync(Guid userId);
    Task<Note> AddAsync(Note note);

    Task<Note?> GetByIdAsync(Guid id);

    Task UpdateAsync(Note note);
    Task DeleteAsync(Guid id);
}