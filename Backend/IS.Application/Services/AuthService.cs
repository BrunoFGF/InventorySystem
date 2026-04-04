using IS.Application.DTOs.Auth;
using IS.Application.Interfaces;
using IS.Domain.Exceptions;
using IS.Domain.Interfaces;
using IS.Shared.Constants;

namespace IS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthService(IUnitOfWork unitOfWork, IJwtGenerator jwtGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new BusinessException(ErrorMessages.InvalidCredentials);

            return _jwtGenerator.GenerateToken(user);
        }
    }
}