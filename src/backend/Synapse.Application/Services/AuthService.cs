using System.Security.Claims;
using System.Text;
using Synapse.Domain.Entities;
using Synapse.Application.DTOs;
using Synapse.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Synapse.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, JwtSettings jwtSettings)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings;
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepository.ExistsAsync(dto.Email)) return "Email already in use";

        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        };
        await _userRepository.AddAsync(user);
        return GenerateJwt(user);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        return GenerateJwt(user);
    }

    private string GenerateJwt(User user)
    {

        var key = _jwtSettings.Key;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) //for siganlR claims
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}