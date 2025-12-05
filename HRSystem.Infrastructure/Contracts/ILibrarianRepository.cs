// HRSystem.Infrastructure.Contracts/ILibrarianRepository.cs

using System.Threading.Tasks;
using HRSystem.BaseLibrary.Models;
using System.Collections.Generic;
using HRSystem.Infrastructure.Contracts;
using HRSystem.BaseLibrary.DTOs;

public interface ILibrarianRepository : IGenericRepository<LIBRARIAN>
{
    // ... existing functions

    Task<LIBRARIAN?> GetLibrarianByEmailAsync(string email);
}