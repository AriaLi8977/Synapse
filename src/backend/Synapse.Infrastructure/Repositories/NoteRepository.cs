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

    public async Task<Note?> GetByIdAsync(Guid id)
    {
        return await _db.Notes
                    .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(Note note)
    {
        var existing = await _db.Notes.FindAsync(note.Id);

        if (existing == null)
            throw new Exception("Note not found");

        existing.Content = note.Content;
        existing.Summary = note.Summary;
        existing.Status = note.Status;
        await _db.SaveChangesAsync();
    }
}