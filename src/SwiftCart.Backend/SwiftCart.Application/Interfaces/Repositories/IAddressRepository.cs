using System;
using SwiftCart.Domain.Entities;

namespace SwiftCart.Application.Interfaces.Repositories;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(Guid id);
    Task<Address> CreateAsync(Address address);
 }
