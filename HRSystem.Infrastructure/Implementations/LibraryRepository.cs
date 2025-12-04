// In HRSystem.Infrastructure/Implementations/LibraryRepository.cs
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Data;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class LibraryRepository : GenericRepository<LIBRARY>, ILibraryRepository
{
    private readonly HRSystemContext _context;

    public LibraryRepository(HRSystemContext context) : base(context)
    {
        _context = context;
    }

    // Implementation of specific method: Get by Tax Number
    public async Task<LIBRARY?> GetLibraryByTaxNumberAsync(string taxNumber)
    {
        return await _context.LIBRARies
            .FirstOrDefaultAsync(l => l.tax_number == taxNumber);
    }
}