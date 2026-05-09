

using Application.DTOs.Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterTeamAsync(RegisterDto registerDto);
    }
}
