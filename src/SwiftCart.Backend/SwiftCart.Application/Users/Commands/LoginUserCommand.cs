using System;
using MediatR;
using SwiftCart.Application.Interfaces.Repositories;

namespace SwiftCart.Application.Users.Commands;

public class LoginUserCommand : IRequest<string>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public class Handler(IUserRepository repo, IJwtTokenGenerator tokenGenerator) : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _repo = repo;
        private readonly IJwtTokenGenerator _jwtGenerator = tokenGenerator;

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var passwordMatches = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordMatches)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = _jwtGenerator.GenerateToken(user);
            return token;
        }
    }
}
