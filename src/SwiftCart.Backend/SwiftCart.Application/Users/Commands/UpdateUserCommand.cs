using System;
using MediatR;
using SwiftCart.Application.Dto;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Users.Commands;

public class UpdateUserCommand : IRequest<bool>
{
	public string Email { get; set; } = string.Empty;
	public UpdateUserDto? User { get; set; }

	public class Handler(IUserRepository repo) : IRequestHandler<UpdateUserCommand, bool>
	{
		private readonly IUserRepository _repo = repo;

		public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
		{
			var existing = await _repo.GetUserByEmailAsync(request.Email);
			if (existing == null) throw new KeyNotFoundException("User not found.");

			var user = request.User;
			if (user == null) return false;

			if (!string.IsNullOrWhiteSpace(user.Username))
				existing.Username = user.Username;

			if (user.Addresses != null && user.Addresses.Count > 0)
			{
				foreach (var addr in user.Addresses)
				{
					if (addr.Id == null || addr.Id == Guid.Empty)
					{
						var newAddress = new Address
						{
							UserId = existing.Id,
							FullName = addr.FullName,
							Street = addr.Street,
							City = addr.City,
							State = addr.State,
							ZipCode = addr.ZipCode,
							Country = addr.Country,
							IsDefault = addr.IsDefault ?? false
						};
						existing.Addresses.Add(newAddress);
					}
					else
					{
						var existingAddress = existing.Addresses.FirstOrDefault(a => a.Id == addr.Id);
						if (existingAddress != null)
						{
							if (!string.IsNullOrWhiteSpace(addr.FullName)) existingAddress.FullName = addr.FullName;
							if (!string.IsNullOrWhiteSpace(addr.Street)) existingAddress.Street = addr.Street;
							if (!string.IsNullOrWhiteSpace(addr.City)) existingAddress.City = addr.City;
							if (!string.IsNullOrWhiteSpace(addr.State)) existingAddress.State = addr.State;
							if (!string.IsNullOrWhiteSpace(addr.ZipCode)) existingAddress.ZipCode = addr.ZipCode;
							if (!string.IsNullOrWhiteSpace(addr.Country)) existingAddress.Country = addr.Country;
							if (addr.IsDefault.HasValue) existingAddress.IsDefault = addr.IsDefault.Value;
						}
					}
				}
			}

			var result = await _repo.SaveChangeAsync();
			return result;
		}
	}
}
