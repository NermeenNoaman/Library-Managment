// HRSystem.Infrastructure.Contracts/ILibrarianService.cs

using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using System.Threading.Tasks;

public interface ILibrarianService
{
    // Update Librarian (Complex logic for linking user/member entities)
    Task<LIBRARIAN> UpdateLibrarianAsync(int librarianId, LibrarianUpdateDto dto);

    // Delete Librarian (Complex logic for cascading delete of USER/MEMBER)
    Task DeleteLibrarianAsync(int librarianId);

    // Get Details (Can be handled by Repo)
    Task<LIBRARIAN?> GetLibrarianDetailsAsync(int librarianId);
}