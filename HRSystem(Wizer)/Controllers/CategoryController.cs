using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRSystem_Wizer_.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // -------------------------------------------------------------------
    // 1. GET (Read All)
    // -------------------------------------------------------------------

    // GET: api/Category
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryReadDto>))]
    [AllowAnonymous] // 🎯 All roles can view
    public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetCategories()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var categoryDtos = _mapper.Map<IEnumerable<CategoryReadDto>>(categories);
        return Ok(categoryDtos);
    }

    // -------------------------------------------------------------------
    // 2. GET (Read Single)
    // -------------------------------------------------------------------

    // GET: api/Category/5
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(CategoryReadDto))]
    [ProducesResponseType(404)]
    [AllowAnonymous] // 🎯 All roles can view by ID
    public async Task<ActionResult<CategoryReadDto>> GetCategory(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound($"Category with ID {id} not found.");
        }

        var categoryDto = _mapper.Map<CategoryReadDto>(category);
        return Ok(categoryDto);
    }

    // -------------------------------------------------------------------
    // 3. POST (Create)
    // -------------------------------------------------------------------

    // POST: api/Category
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(CategoryReadDto))]
    [ProducesResponseType(400)]
    [Authorize(Roles = "Admin,Librarian")] // 🎯 Restricted to Admin and Librarian
    public async Task<ActionResult<CategoryReadDto>> PostCategory([FromBody] CategoryCreateUpdateDto categoryDto)
    {
        var existingCategory = await _unitOfWork.Categories.GetByNameAsync(categoryDto.CategoryName);
        if (existingCategory != null)
        {
            return BadRequest($"Category name '{categoryDto.CategoryName}' already exists.");
        }

        var category = _mapper.Map<CATEGORY>(categoryDto);

        await _unitOfWork.Categories.AddAsync(category);

        await _unitOfWork.CompleteAsync();

        var readDto = _mapper.Map<CategoryReadDto>(category);

        return CreatedAtAction(nameof(GetCategory), new { id = readDto.CategoryId }, readDto);
    }

    // -------------------------------------------------------------------
    // 4. PUT (Update)
    // -------------------------------------------------------------------

    // PUT: api/Category/5
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [Authorize(Roles = "Admin,Librarian")] // 🎯 Restricted to Admin and Librarian
    public async Task<IActionResult> PutCategory(int id, [FromBody] CategoryCreateUpdateDto categoryDto)
    {
        var categoryToUpdate = await _unitOfWork.Categories.GetByIdAsync(id);

        if (categoryToUpdate == null)
        {
            return NotFound($"Category with ID {id} not found.");
        }

        var existingCategory = await _unitOfWork.Categories.GetByNameAsync(categoryDto.CategoryName);
        if (existingCategory != null && existingCategory.category_id != id)
        {
            return BadRequest($"Category name '{categoryDto.CategoryName}' already used by another category.");
        }

        _mapper.Map(categoryDto, categoryToUpdate);


        try
        {
            _unitOfWork.Categories.UpdateAsync(categoryToUpdate);
            await _unitOfWork.CompleteAsync();

            await _unitOfWork.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException) when (!CategoryExists(id))
        {
            return NotFound($"Category with ID {id} not found during update check.");
        }

        return NoContent();
    }

    // -------------------------------------------------------------------
    // 5. DELETE
    // -------------------------------------------------------------------

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [Authorize(Roles = "Admin,Librarian")] // 🎯 Restricted to Admin and Librarian
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var categoryToDelete = await _unitOfWork.Categories.GetByIdAsync(id);

        if (categoryToDelete == null)
        {
            return NotFound($"Category with ID {id} not found.");
        }

        _unitOfWork.Categories.DeleteAsync(categoryToDelete);

        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    // -------------------------------------------------------------------
    // Helper Method
    // -------------------------------------------------------------------

    private bool CategoryExists(int id)
    {
        return _unitOfWork.Categories.GetByIdAsync(id).Result != null;
    }
}



