using Synapse.Application.Interfaces;
using Synapse.Application.DTOs;
using Synapse.Domain.Entities;
using Synapse.Domain.Enums;

namespace Synapse.Application.UseCases;

public class DeleteNoteUseCase
{
    private readonly INoteRepository _noteRep;

    public DeleteNoteUseCase(INoteRepository noteRep)
    {
        _noteRep = noteRep;
    }

    public async Task<bool> ExecuteAsync(Guid id, Guid userId)
    {
        var note = await _noteRep.GetByIdAsync(id);
        if (note == null || note.UserId != userId)
            return false;
        await _noteRep.DeleteAsync(id);
        return true;
    }
}