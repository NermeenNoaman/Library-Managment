using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims; // Required for claims access
using System.Threading.Tasks;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize] // All actions require authentication
public class LibrarianController : ControllerBase // ⚠️ Can be BaseApiController if implemented
{
    private readonly ILibrarianService _service;
    private readonly ILibrarianRepository _repo;
    private readonly IMapper _mapper;

    public LibrarianController(ILibrarianService service, ILibrarianRepository repo, IMapper mapper)
    {
        _service = service;
        _repo = repo;
        _mapper = mapper;
    }

    // -------------------------------------------------------------------------
    // HELPER: Get the current User ID from the JWT Token (To be put in BaseApiController)
    // -------------------------------------------------------------------------
    private int GetCurrentUserId()
    {
        // Check for the claim used to store the User's Primary Key ID (e.g., "user_id" or "UserID")
        var userIdClaim = User.FindFirst("user_id")?.Value;

        if (!int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("User ID claim is missing or invalid in the token.");
        }
        return userId;
    }


    // =========================================================================
    // POST: Create New Librarian (ADMIN only)
    // =========================================================================
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LibrarianReadDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLibrarian([FromBody] LibrarianCreateDto dto)
    {
        try
        {
            var existingLibrarian = await _repo.GetLibrarianByEmailAsync(dto.Email);
            if (existingLibrarian != null)
            {
                return BadRequest(new { Error = "A Librarian with this email already exists." });
            }

            var librarian = _mapper.Map<LIBRARIAN>(dto);

            librarian.status = "Active"; 
            librarian.created_at = DateTime.UtcNow;

            await _repo.AddAsync(librarian);
            await _repo.SaveChangesAsync();

            var readDto = _mapper.Map<LibrarianReadDto>(librarian);

            return CreatedAtAction(nameof(GetLibrarianById), new { id = readDto.LibrarianId }, readDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }




    // =========================================================================
    // GET: All Librarians (Admin access only)
    // =========================================================================
    [HttpGet]
    [Authorize(Roles = "Admin")] // Only Admin can view all
    public async Task<IActionResult> GetAllLibrarians()
    {
        var entities = await _repo.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<LibrarianReadDto>>(entities);
        return Ok(dtos);
    }


    // =========================================================================
    // GET: Get Librarian by Email (Admin only)
    // =========================================================================
    [HttpGet("email/{email}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetLibrarianByEmail(string email)
    {
        var librarian = await _repo.GetLibrarianByEmailAsync(email);
        if (librarian == null) return NotFound($"Librarian with email {email} not found.");

        var dto = _mapper.Map<LibrarianReadDto>(librarian);
        return Ok(dto);
    }

    // =========================================================================
    // GET: Get Librarian by ID (Required by CreatedAtAction)
    // =========================================================================
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Librarian")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibrarianReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetLibrarianById(int id)
    {

        var librarian = await _repo.GetByIdAsync(id);
        if (librarian == null) return NotFound();

        if (User.IsInRole("Librarian") && !User.IsInRole("Admin"))
        {
            int currentUserId = GetCurrentUserId();

           
        }

        var dto = _mapper.Map<LibrarianReadDto>(librarian);
        return Ok(dto);
    }

    // =========================================================================
    // PUT: Update Librarian (Admin/Self-Access)
    // =========================================================================
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateLibrarian(int id, [FromBody] LibrarianUpdateDto dto)
    {
        // Security Check: Only Admin can update any ID. Librarian can only update their own.
        if (User.IsInRole("Librarian") && !User.IsInRole("Admin"))
        {
            int currentUserId = GetCurrentUserId();
            var librarian = await _repo.GetByIdAsync(id);

            
            // Librarians cannot update their own status/salary unless Admin
            if (dto.Status != null || dto.Salary.HasValue)
            {
                return BadRequest("Librarians cannot update status or salary.");
            }
        }

        try
        {
            var updatedLibrarian = await _service.UpdateLibrarianAsync(id, dto);
            var readDto = _mapper.Map<LibrarianReadDto>(updatedLibrarian);

            return Ok(readDto);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("not found")) return NotFound(new { Error = ex.Message });
            return BadRequest(new { Error = ex.Message });
        }
    }

    // =========================================================================
    // DELETE: Delete Librarian (Admin only)
    // =========================================================================
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteLibrarian(int id)
    {
        try
        {
            // Use Service for complex delete logic (USER and LIBRARIAN)
            await _service.DeleteLibrarianAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}