using HRSystem.BaseLibrary.DTOs;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

[Route("api/v1/[controller]")]
[ApiController]
// Base authorization applied to the entire controller: Requires any authenticated user.
[Authorize]
public class BookController : ControllerBase
{
    private readonly IBookService _service;

    public BookController(IBookService service)
    {
        _service = service;
    }

    // =========================================================================
    // 1. POST: Create New Book (Admin, Librarian)
    // =========================================================================
    [HttpPost]
    [Authorize(Roles = "Admin,Librarian")] // Restricted to inventory managers
    public async Task<IActionResult> CreateBook([FromBody] BookCreateDto dto)
    {
        try
        {
            var createdBook = await _service.CreateBookAsync(dto);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.BookId }, createdBook);
        }
        catch (Exception ex)
        {
            // Handles ISBN duplication or other business logic errors
            return BadRequest(new { Error = ex.Message });
        }
    }

    // =========================================================================
    // 2. GET: Get All Books (General Access)
    // =========================================================================
    [HttpGet]
    [AllowAnonymous] // 🎯 All roles (Member, Librarian, Admin) can view all books
    public async Task<IActionResult> GetAll()
    {
        var books = await _service.GetAllBooksAsync();
        // Returns 200 OK, even if the list is empty
        return Ok(books);
    }

    // =========================================================================
    // 3. GET: Get Book by ID (General Access)
    // =========================================================================
    [HttpGet("{id}")]
    [AllowAnonymous] // 🎯 All roles can view a specific book by ID
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _service.GetBookByIdAsync(id);
        if (book == null) return NotFound($"Book with ID {id} not found.");
        return Ok(book);
    }

    // =========================================================================
    // 4. GET: Search Books by Title (General Access)
    // =========================================================================
    [HttpGet("search")]
    [AllowAnonymous] // 🎯 All roles can search by title
    public async Task<IActionResult> SearchByTitle([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title search keyword is required.");

        var books = await _service.SearchBooksByTitleAsync(title);
        // Ensure to return 404 if no results are found
        if (!books.Any()) return NotFound($"No books found matching the title '{title}'.");

        return Ok(books);
    }

    // =========================================================================
    // 5. GET: Get Books by Category ID (General Access)
    // =========================================================================
    [HttpGet("category/{categoryId}")]
    [AllowAnonymous] // 🎯 All roles can filter by category
    public async Task<IActionResult> GetBooksByCategory(int categoryId)
    {
        if (categoryId <= 0)
            return BadRequest("Invalid category ID.");

        var books = await _service.GetBooksByCategoryIdAsync(categoryId);

        if (!books.Any())
        {
            return NotFound($"No books found for Category ID {categoryId}.");
        }

        return Ok(books);
    }

    // =========================================================================
    // 6. PUT: Update Book (Admin, Librarian)
    // =========================================================================
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Librarian")] // Restricted to inventory managers
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto dto)
    {
        var result = await _service.UpdateBookAsync(id, dto);
        if (!result) return NotFound($"Book with ID {id} not found.");
        return NoContent(); // 204 Success (Standard for successful update without content)
    }

    // =========================================================================
    // 7. DELETE: Delete Book (Admin, Librarian)
    // =========================================================================
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Librarian")] // Restricted to inventory managers
    public async Task<IActionResult> DeleteBook(int id)
    {
        var result = await _service.DeleteBookAsync(id);
        if (!result) return NotFound($"Book with ID {id} not found.");
        return NoContent(); // 204 Success
    }
}