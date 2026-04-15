using Microsoft.AspNetCore.Mvc;
using Synapse.Application.DTOs;
using Synapse.Domain.Entities;
using Synapse.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto){
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        var token = await _authService.RegisterAsync(dto);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto){
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        var token = await _authService.LoginAsync(dto);
        return Ok(token);
    }
    
}