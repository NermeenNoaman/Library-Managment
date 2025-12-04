// In HRSystem.Infrastructure/Implementations/LibraryBranchRepository.cs
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using HRSystem.Infrastructure.Implementations;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class LibraryBranchRepository : GenericRepository<LIBRARY_BRANCH>, ILibraryBranchRepository
{
    private readonly HRSystemContext _context;

    public LibraryBranchRepository(HRSystemContext context) : base(context)
    {
        _context = context;
    }

    // Implementation of specific method: Get by Branch Name
    public async Task<LIBRARY_BRANCH?> GetBranchByNameAsync(string branchName)
    {
        return await _context.LIBRARY_BRANCHes
            .FirstOrDefaultAsync(b => b.branch_name == branchName);
    }

    // Implementation of specific method: Get branches by Library ID
    public async Task<IEnumerable<LIBRARY_BRANCH>> GetBranchesByLibraryIdAsync(int libraryId)
    {
        return await _context.LIBRARY_BRANCHes
            .Where(b => b.library_id == libraryId)
            .ToListAsync();
    }
}