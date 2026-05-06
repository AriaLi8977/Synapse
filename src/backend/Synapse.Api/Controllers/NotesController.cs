using Microsoft.AspNetCore.Mvc;
using Synapse.Application.DTOs;
using Synapse.Domain.Entities;
using Synapse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Synapse.Api.Controllers;

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

    [Authorize]
    [HttpGet("GetNotes")]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var notes = await _noteService.GetAllAsync(Guid.Parse(userId));
        return Ok(notes);
    }

    [Authorize]
    [HttpPost("CreateNotes")]
    public async Task<IActionResult> Create(CreateNoteDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _useCase.ExecuteAsync(content);
        return Ok(result);
    }
}