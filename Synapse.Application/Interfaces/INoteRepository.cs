using Synapse.Domain.Entities;

public interface INoteRepository{
    Task<List<Note>> GetAllAsync(Guid userId);
    Task<Note> AddAsync(Note note, Guid userId);
}