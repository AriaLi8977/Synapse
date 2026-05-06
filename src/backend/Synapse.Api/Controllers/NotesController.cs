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
    private readonly CreateNoteUseCase _useCase;

    public NotesController(INoteService noteService,
                            CreateNoteUseCase useCase)
    {
        _noteService = noteService;
        _useCase = useCase;
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
        var result = await _useCase.ExecuteAsync(dto.Content, Guid.Parse(userId));
        return Ok(result);
    }
}