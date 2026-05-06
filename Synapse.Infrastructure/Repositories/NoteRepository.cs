using Synapse.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Synapse.Infrastructure.Data;
using Synapse.Application.Interfaces;

public class NoteRepository : INoteRepository
{
    private readonly AppDbContext _db;

    public NoteRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Note>> GetAllAsync(Guid userId)
    {
        return await _db.Notes
                .Where(x => x.UserId == userId)
                .ToListAsync();
    }

    public async Task<Note> AddAsync(Note note)
    {
        _db.Notes.Add(note);
        await _db.SaveChangesAsync();
        return note;
    }
}