
namespace Synapse.Application.UseCases;

public class DeleteNoteUseCase
{
    private readonly INoteRepository _noteRep;

    public DeleteNoteUseCase(INoteRepository noteRep)
    {
        _noteRep = noteRep;
    }

    public async Task ExecuteAsync(Guid id, Guid userId)
    {
        var note = await _noteRep.GetByIdAsync(id);
        if (note == null || note.UserId != userId)
            throw new Exception("Note not found or access denied");
        await _noteRep.DeleteAsync(id);
    }
}