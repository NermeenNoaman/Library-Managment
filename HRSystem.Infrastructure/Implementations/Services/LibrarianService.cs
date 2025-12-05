// HRSystem.Infrastructure.Implementations.Services/LibrarianService.cs

using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using System.Threading.Tasks;
using System;
using HRSystem.Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;

public class LibrarianService : ILibrarianService
{
    private readonly ILibrarianRepository _repo;
    private readonly HRSystemContext _context; 

    public LibrarianService(ILibrarianRepository repo, HRSystemContext context)
    {
        _repo = repo;
        _context = context;
    }

   

    // =======================================================
    // 2. Update Librarian
    // =======================================================
    public async Task<LIBRARIAN> UpdateLibrarianAsync(int librarianId, LibrarianUpdateDto dto)
    {
        var librarian = await _repo.GetByIdAsync(librarianId);
        if (librarian == null)
        {
            throw new Exception("Librarian not found.");
        }

        // 🛑 منطق التحديث (بما أننا لا نستخدم AutoMapper في Service)
        if (dto.FirstName != null) librarian.first_name = dto.FirstName;
        if (dto.LastName != null) librarian.last_name = dto.LastName;
        if (dto.Phone != null) librarian.phone = dto.Phone;
        if (dto.Salary.HasValue) librarian.salary = dto.Salary.Value;
        if (dto.Status != null) librarian.status = dto.Status;

        librarian.updated_at = DateTime.UtcNow;

       

        await _repo.UpdateAsync(librarian);
        await _repo.SaveChangesAsync();

        return librarian;
    }

    // =======================================================
    // 3. Get Librarian Details
    // =======================================================
    public async Task<LIBRARIAN?> GetLibrarianDetailsAsync(int librarianId)
    {
        return await _repo.GetByIdAsync(librarianId);
    }

    // HRSystem.Infrastructure.Implementations.Services/LibrarianService.cs
    // ... (Constructor and other methods are assumed to be correct)

    // =======================================================
    // 4. Delete Librarian (Requires cascading delete of USER) - NEW
    // =======================================================
    public async Task DeleteLibrarianAsync(int librarianId)
    {
        var librarian = await _repo.GetByIdAsync(librarianId);
        if (librarian == null) return;

        // Find and delete the linked USER account
      

        // Remove the Librarian entity
        _context.LIBRARIANs.Remove(librarian);

        // Save both changes in one go
        await _context.SaveChangesAsync();
    }

}