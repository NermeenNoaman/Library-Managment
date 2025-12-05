using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[Authorize]
[ApiController]

public class BorrowingController : ControllerBase
{
    private readonly IBorrowingRepository _repo;
    private readonly IBorrowingService _service;
    private readonly IMapper _mapper;
    private readonly IMemberRepository _memberRepo;

    public BorrowingController(IBorrowingRepository repo, IBorrowingService service, IMapper mapper, IMemberRepository memberRepo)
    {
        _repo = repo;
        _service = service;
        _mapper = mapper;
        _memberRepo = memberRepo;
    }

    // -------------------------------------------------------------------------
    // HELPER: Get the current USER ID from the JWT Token 
    // -------------------------------------------------------------------------
    private int GetCurrentUserIdFromToken() // 👈 تغيير اسم الدالة للتوضيح
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("User ID claim is missing or invalid in the token.");
        }
        return userId;
    }

    // =========================================================================
    // 1. GET: Get My Borrowings (Self-Service) - REQUESTED ENDPOINT
    // =========================================================================
    [HttpGet("my")]
    [Authorize(Roles = "Member")] // Only the member can view their own borrowings
    public async Task<IActionResult> GetMyBorrowings()
    {
        try
        {
            // 1. Get User ID from Token
            int userId = GetCurrentUserIdFromToken();

            // 2. Map User ID (Token ID) to Member ID (Database FK)
            int? memberId = await _memberRepo.GetMemberIdByUserIdAsync(userId);

            if (memberId == null || memberId.Value == 0)
            {
                // If user is authenticated but has no associated Member record
                return NotFound("Member profile not found or not fully registered.");
            }

            // 3. Fetch active borrowings for that Member ID
            var entities = await _repo.GetBorrowingsByMemberIdAsync(memberId.Value);

            if (!entities.Any())
            {
                return NotFound("No active borrowings found for the current member.");
            }

            var dtos = _mapper.Map<IEnumerable<BorrowingReadDto>>(entities);
            return Ok(dtos);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Authentication failed or token is invalid.");
        }
        catch (Exception ex)
        {
            // Catch general errors (e.g., database issues)
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
        }
    }


    // =========================================================================
    // POST: Borrow Book (Simplified Self-Service)
    // =========================================================================
    [HttpPost("borrow/{bookId}")]
    [Authorize(Roles = "Member")]
    public async Task<IActionResult> BorrowBook(int bookId)
    {
        try
        {
            // 1. استخراج الـ USER ID من التوكن
            int userId = GetCurrentUserIdFromToken();

            // 2. البحث عن الـ MEMBER ID الفعلي
            int? memberId = await _memberRepo.GetMemberIdByUserIdAsync(userId);

            if (memberId == null )
            {
                // 🎯 إذا كان User موجودًا ولكنه غير مرتبط بملف Member (هذا هو سبب الفشل)
                return BadRequest(new { Error = "User account is not associated with an active Member profile." });
            }

            // 3. إنشاء DTO مؤقت لمتطلبات الـ Service
            var inputDto = new BorrowingCreateDto
            {
                MemberId = memberId.Value, // استخدام القيمة الفعلية
                BookId = bookId
            };

            // 4. استدعاء الـ Service
            var resultDto = await _service.BorrowBookAsync(inputDto);

            return Ok(new
            {
                Message = "Book successfully borrowed.",
                Borrowing = resultDto
            });
        }
        catch (UnauthorizedAccessException)
        {
            // Unauthorized (should be handled by middleware, but kept for clarity)
            return Unauthorized("Authentication required or token is invalid.");
        }
        catch (Exception ex)
        {
            // DbUpdateException/FK Constraint Violation (سيظهر هنا إذا كان هناك مشكلة في البيانات)
            return BadRequest(new { Error = ex.Message });
        }
    }

    // =========================================================================
    // POST: Return Book
    // =========================================================================
    [HttpPost("return/{id}")]
    [Authorize(Roles = "Admin,Librarian")]
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
    [Authorize(Roles = "Admin,Librarian")]

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
    [Authorize(Roles = "Member")]

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
    [Authorize(Roles = "Admin,Librarian")]

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
    [Authorize(Roles = "Admin,Librarian")]

    public async Task<IActionResult> DeleteBorrowing(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();

        return NoContent();
    }
}
