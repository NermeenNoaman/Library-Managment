// In HRSystem(Wizer)/Controllers/LibraryBranchController.cs
using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")] // Admin and Librarians manage branches
public class LibraryBranchController : ControllerBase
{
    private readonly ILibraryBranchRepository _branchRepo;
    // Assuming you have an IBranchRepository for the main Library entity validation
    private readonly ILibraryRepository _libraryRepo;
    private readonly IMapper _mapper;

    public LibraryBranchController(ILibraryBranchRepository branchRepo, ILibraryRepository libraryRepo, IMapper mapper)
    {
        _branchRepo = branchRepo;
        _libraryRepo = libraryRepo;
        _mapper = mapper;
    }

    // =========================================================================
    // 1. POST: Create New Branch (CREATE)
    // =========================================================================
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LibraryBranchReadDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")] // Admin and Librarians manage branches

    public async Task<IActionResult> CreateBranch([FromBody] LibraryBranchCreateDto dto)
    {
        // Validation: Check if the parent Library exists
        var parentLibrary = await _libraryRepo.GetByIdAsync(dto.LibraryId);
        if (parentLibrary == null)
        {
            return NotFound(new { Message = $"Parent Library with ID {dto.LibraryId} not found." });
        }

        // Validation: Check for unique Branch Name
        var existing = await _branchRepo.GetBranchByNameAsync(dto.BranchName);
        if (existing != null)
        {
            return Conflict(new { Message = $"Branch name '{dto.BranchName}' already exists." });
        }

        var entity = _mapper.Map<LIBRARY_BRANCH>(dto);

        var createdEntity = await _branchRepo.AddAsync(entity);
        await _branchRepo.SaveChangesAsync();

        var createdDto = _mapper.Map<LibraryBranchReadDto>(createdEntity);
        return CreatedAtAction(nameof(GetBranchById), new { id = createdDto.BranchId }, createdDto);
    }

    // =========================================================================
    // 2. GET: Get Branch by ID (READ SINGLE)
    // =========================================================================
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LibraryBranchReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")] // Admin and Librarians manage branches

    public async Task<IActionResult> GetBranchById(int id)
    {
        var entity = await _branchRepo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        var dto = _mapper.Map<LibraryBranchReadDto>(entity);
        return Ok(dto);
    }

    // =========================================================================
    // 3. GET: Get All Branches (READ ALL)
    // =========================================================================
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LibraryBranchReadDto>))]
    [Authorize(Roles = "Admin")] // Admin and Librarians manage branches

    public async Task<IActionResult> GetAllBranches()
    {
        var entities = await _branchRepo.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<LibraryBranchReadDto>>(entities);
        return Ok(dtos);
    }

    // =========================================================================
    // 4. PUT: Update Branch Details (UPDATE)
    // =========================================================================
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")] // Admin and Librarians manage branches

    public async Task<IActionResult> UpdateBranch(int id, [FromBody] LibraryBranchUpdateDto dto)
    {
        if (id != dto.BranchId)
        {
            return BadRequest(new { Message = "ID mismatch between route and body." });
        }

        var existingEntity = await _branchRepo.GetByIdAsync(id);
        if (existingEntity == null) return NotFound();

        // Apply partial updates
        _mapper.Map(dto, existingEntity);

        await _branchRepo.UpdateAsync(existingEntity);
        await _branchRepo.SaveChangesAsync();

        return Ok(new { Message = "Branch record updated successfully." });
    }

    // =========================================================================
    // 5. DELETE: Delete Branch Record (DELETE)
    // =========================================================================
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")] // Admin and Librarians manage branches

    public async Task<IActionResult> DeleteBranch(int id)
    {
        var entity = await _branchRepo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        await _branchRepo.DeleteAsync(entity);
        await _branchRepo.SaveChangesAsync();

        return NoContent();
    }
}