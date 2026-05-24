using Microsoft.AspNetCore.Mvc;
using Synapse.Application.DTOs;
using Synapse.Domain.Entities;
using Synapse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Synapse.Application.UseCases;


namespace Synapse.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/Notes")]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;
    private readonly CreateNoteUseCase _createUseCase;
    private readonly DeleteNoteUseCase _deleteUseCase;

    public NotesController(INoteService noteService,
                            CreateNoteUseCase createUseCase,
                            DeleteNoteUseCase deleteUseCase)
    {
        _noteService = noteService;
        _createUseCase = createUseCase;
        _deleteUseCase = deleteUseCase;
    }


    [HttpGet("GetNotes")]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        var notes = await _noteService.GetAllAsync(Guid.Parse(userId));
        return Ok(notes);
    }

    [HttpPost("CreateNotes")]
    public async Task<IActionResult> Create(CreateNoteDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        var note = await _createUseCase.ExecuteAsync(dto.Content, Guid.Parse(userId));
        return Ok(note);
    }

    [HttpGet("GetNoteById/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        var note = await _noteService.GetByIdAsync(id, Guid.Parse(userId));
        if (note == null)
            return NotFound();
        return Ok(note);
    }

    // [HttpPut("UpdateNote/{id}")]
    // public async Task<IActionResult> Update(Guid id, UpdateNoteDto dto)
    // {
    //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     if (userId == null)
    //         return Unauthorized();
    //     var note = await _noteService.GetByIdAsync(id, Guid.Parse(userId));
    //     if (note == null)
    //         return NotFound();
    //     var result = await _noteService.UpdateAsync(id, dto, Guid.Parse(userId));
    //     if (result)
    //     {
    //         return Ok(new { Message = "Note updated successfully." });
    //     }
    //     else
    //     {
    //         return BadRequest(new { Message = "Failed to update the note." });
    //     }
    // }


    [HttpDelete("DeleteNote/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var success = await _deleteUseCase.ExecuteAsync(id, Guid.Parse(userId));
        if (success)
        {
            return Ok(new { Message = "Note deleted successfully." });
        }
        else
        {
            return BadRequest(new { Message = "Failed to delete the note." });
        }
    }
}