using System.Security.Claims;
using System.Text;
using Synapse.Domain.Entities;
using Synapse.Application.DTOs;
using Synapse.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace Synapse.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtOptions)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepository.ExistsAsync(dto.Email))
        {
            return new AuthResponseDto
            {
                Success = false,
                Code = "EMAIL_EXISTS",
                Message = "Email already in use."
            };
        }

        var user = new Domain.Entities.User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        };

        await _userRepository.AddAsync(user);
        
        return new AuthResponseDto
        {
            Success = true,
            Code = "Registration Successful",
            Token = GenerateJwt(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return new AuthResponseDto
            {
                Success = false,
                Code = "INVALID_CREDENTIALS",
                Message = "Invalid email or password"
            };
        }

        return new AuthResponseDto
        {
            Success = true,
            Code = "Registration Successful",
            Token = GenerateJwt(user)
        };
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