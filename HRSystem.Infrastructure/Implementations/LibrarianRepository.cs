// HRSystem.Infrastructure.Implementations/LibrarianRepository.cs

using HRSystem.BaseLibrary.Models;
using HRSystem.BaseLibrary.DTOs;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using HRSystem.Infrastructure.Data;
using HRSystem.Infrastructure.Implementations;

public class LibrarianRepository : GenericRepository<LIBRARIAN>, ILibrarianRepository
{
    private readonly HRSystemContext _context;

    public LibrarianRepository(HRSystemContext context) : base(context)
    {
        _context = context;
    }


    

    // =======================================================
    // 1. Get Librarian by Email - NEW
    // =======================================================
    public async Task<LIBRARIAN?> GetLibrarianByEmailAsync(string email)
    {
        // Assuming the LIBRARIAN entity has an 'email' field.
        return await _context.LIBRARIANs.FirstOrDefaultAsync(l => l.email == email);
    }
}