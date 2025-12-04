// In HRSystem.Infrastructure/Contracts/ILibraryBranchRepository.cs
using HRSystem.BaseLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Contracts
{
    // Extends IGenericRepository<T> for standard CRUD
    public interface ILibraryBranchRepository : IGenericRepository<LIBRARY_BRANCH>
    {
        // Logic: Check for unique branch name before creation (UQ constraint)
        Task<LIBRARY_BRANCH?> GetBranchByNameAsync(string branchName);

        // Reporting: Get all branches belonging to a specific library
        Task<IEnumerable<LIBRARY_BRANCH>> GetBranchesByLibraryIdAsync(int libraryId);
    }
}