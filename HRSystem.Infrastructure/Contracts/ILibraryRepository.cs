// In HRSystem.Infrastructure/Contracts/ILibraryRepository.cs
using HRSystem.BaseLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Contracts
{
    // Extends IGenericRepository<T> for standard CRUD
    public interface ILibraryRepository : IGenericRepository<LIBRARY>
    {
        // Add specific methods here if needed, e.g., GetByTaxNumberAsync
        Task<LIBRARY?> GetLibraryByTaxNumberAsync(string taxNumber);
    }
}