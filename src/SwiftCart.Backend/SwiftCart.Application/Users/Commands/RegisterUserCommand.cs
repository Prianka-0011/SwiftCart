using System;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using SwiftCart.Application.Dto;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Application.Mappings;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Users.Commands;

public class RegisterUserCommand : IRequest<UserResponseDto>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
 

    public class Handler(IUserRepository repo) : IRequestHandler<RegisterUserCommand, UserResponseDto>
    {
         private readonly IUserRepository _repo = repo;
        public async Task<UserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
             
            var existing = await _repo.GetUserByEmailAsync(request.Email);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = hashedPassword;
            if (existing != null)
            {
                throw new Exception("User already registered.");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword
            };
            var result = await _repo.CreateUserAsync(user);
            var response = UserMapper.MapToDto(result);
            
            return response;
        }
    }
}
