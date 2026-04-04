using IS.Application.DTOs.Auth;

namespace IS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(LoginDto dto);
    }
}