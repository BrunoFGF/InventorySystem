using IS.Application.DTOs.Auth;
using IS.Application.Interfaces;
using IS.Domain.Exceptions;
using IS.Domain.Interfaces;
using IS.Shared.Constants;
using Microsoft.Extensions.Logging;

namespace IS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, IJwtGenerator jwtGenerator, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _jwtGenerator = jwtGenerator;
            _logger = logger;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for email {Email}", dto.Email);
                throw new BusinessException(ErrorMessages.InvalidCredentials);
            }

            _logger.LogInformation("User {UserId} ({Email}) logged in successfully", user.Id, user.Email);
            return _jwtGenerator.GenerateToken(user);
        }
    }
}