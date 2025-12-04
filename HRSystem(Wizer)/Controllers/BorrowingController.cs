using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class BorrowingController : ControllerBase
{
    private readonly IBorrowingRepository _repo;
    private readonly IBorrowingService _service;
    private readonly IMapper _mapper;

    public BorrowingController(IBorrowingRepository repo, IBorrowingService service, IMapper mapper)
    {
        _repo = repo;
        _service = service;
        _mapper = mapper;
    }

    // =========================================================================
    // POST: Borrow Book
    // =========================================================================
    [HttpPost("borrow")]
    public async Task<IActionResult> BorrowBook([FromBody] BorrowingCreateDto dto)
    {
        var borrowing = await _service.BorrowBookAsync(dto);
        var readDto = _mapper.Map<BorrowingReadDto>(borrowing);
        return CreatedAtAction(nameof(GetBorrowingById), new { id = readDto.BorrowingId }, readDto);
    }

    // =========================================================================
    // POST: Return Book
    // =========================================================================
    [HttpPost("return/{id}")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var borrowing = await _service.ReturnBookAsync(id);
        var readDto = _mapper.Map<BorrowingReadDto>(borrowing);
        return Ok(readDto);
    }

    // =========================================================================
    // GET: All Borrowings
    // =========================================================================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entities = await _repo.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<BorrowingReadDto>>(entities);
        return Ok(dtos);
    }

    // =========================================================================
    // GET: Borrowing by ID
    // =========================================================================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBorrowingById(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        var dto = _mapper.Map<BorrowingReadDto>(entity);
        return Ok(dto);
    }

    // =========================================================================
    // PUT: Update Borrowing
    // =========================================================================
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBorrowing(int id, [FromBody] BorrowingUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        _mapper.Map(dto, entity);
        await _repo.UpdateAsync(entity);
        await _repo.SaveChangesAsync();

        return Ok(new { Message = "Borrowing updated successfully." });
    }

    // =========================================================================
    // DELETE: Delete Borrowing
    // =========================================================================
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBorrowing(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();

        return NoContent();
    }
}
