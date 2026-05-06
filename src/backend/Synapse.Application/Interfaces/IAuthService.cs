using Synapse.Application.DTOs;

public interface IAuthService{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto); 
}