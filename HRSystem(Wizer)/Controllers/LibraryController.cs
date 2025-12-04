// In HRSystem(Wizer)/Controllers/LibraryController.cs
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
//[Authorize(Roles = "Admin")] // Only Admin should manage master library data
public class LibraryController : ControllerBase
{
    private readonly ILibraryRepository _libraryRepo;
    private readonly IMapper _mapper;

    public LibraryController(ILibraryRepository libraryRepo, IMapper mapper)
    {
        _libraryRepo = libraryRepo;
        _mapper = mapper;
    }

    // =========================================================================
    // POST: Create New Library Record (CREATE)
    // =========================================================================
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LibraryReadDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateLibrary([FromBody] LibraryCreateDto dto)
    {
        // Validation: Check for unique Tax Number
        var existing = await _libraryRepo.GetLibraryByTaxNumberAsync(dto.TaxNumber);
        if (existing != null)
        {
            return Conflict(new { Message = $"Library with tax number '{dto.TaxNumber}' already exists." });
        }

        var entity = _mapper.Map<LIBRARY>(dto);

        var createdEntity = await _libraryRepo.AddAsync(entity);
        await _libraryRepo.SaveChangesAsync();

        var createdDto = _mapper.Map<LibraryReadDto>(createdEntity);
        return CreatedAtAction(nameof(GetLibraryById), new { id = createdDto.LibraryId }, createdDto);
    }

    // =========================================================================
    // GET: Get All Libraries (READ ALL)
    // =========================================================================
    [HttpGet]
    public async Task<IActionResult> GetAllLibraries()
    {
        var entities = await _libraryRepo.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<LibraryReadDto>>(entities);
        return Ok(dtos);
    }

    // =========================================================================
    // GET: Get Library by ID (READ SINGLE)
    // =========================================================================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLibraryById(int id)
    {
        var entity = await _libraryRepo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        var dto = _mapper.Map<LibraryReadDto>(entity);
        return Ok(dto);
    }

    // =========================================================================
    // PUT: Update Library Details (UPDATE)
    // =========================================================================
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLibrary(int id, [FromBody] LibraryUpdateDto dto)
    {
        var existingEntity = await _libraryRepo.GetByIdAsync(id);
        if (existingEntity == null) return NotFound();

        // Apply partial updates
        _mapper.Map(dto, existingEntity);

        await _libraryRepo.UpdateAsync(existingEntity);
        await _libraryRepo.SaveChangesAsync();

        return Ok(new { Message = "Library record updated successfully." });
    }

    // =========================================================================
    // DELETE: Delete Library Record (DELETE)
    // =========================================================================
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLibrary(int id)
    {
        var entity = await _libraryRepo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        // Note: Hard Delete is used here; use Soft Delete if required by project rules.
        await _libraryRepo.DeleteAsync(entity);
        await _libraryRepo.SaveChangesAsync();

        return NoContent();
    }
}