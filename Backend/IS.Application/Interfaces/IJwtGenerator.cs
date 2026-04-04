using IS.Application.DTOs.Auth;
using IS.Domain.Entities;

namespace IS.Application.Interfaces
{
    public interface IJwtGenerator
    {
        TokenResponseDto GenerateToken(User user);
    }
}