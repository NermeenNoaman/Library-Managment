using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/v1/[controller]")] // Adjusted route for consistency
[ApiController]
[Authorize] // Base authentication required
public class FineController : ControllerBase
{
    private readonly IFineService _service;
    private readonly IMapper _mapper;

    // Note: Assuming ILogger is injected here as well, if needed.
    public FineController(IFineService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // -------------------------------------------------------------------------
    // HELPER: Get the current Member ID from the JWT Token 
    // -------------------------------------------------------------------------
    private int GetCurrentUserId()
    {
        // ⚠️ Assuming the Primary Key ID claim is stored under ClaimTypes.NameIdentifier
        var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out int currentUserId))
        {
            // If the standard NameIdentifier claim is missing, try a custom one like "user_id"
            currentUserIdClaim = User.FindFirst("user_id");
            if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out currentUserId))
                throw new UnauthorizedAccessException("User ID claim is missing or invalid in the token.");
        }
        return currentUserId;
    }

    // =========================================================================
    // 1. GET: Fines by Member ID (Admin, Librarian, Self-Access Member)
    // =========================================================================
    [HttpGet("member/{memberId}")]
    [Authorize(Roles = "Admin,Librarian,Member")] // All roles need access
    public async Task<IActionResult> GetFinesForMember(int memberId)
    {
        var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        int currentUserId = GetCurrentUserId();

        // 🛡️ RBAC: If the user is a Member, they can only view their own fines
        if (currentUserRole == "Member" && currentUserId != memberId)
        {
            return Forbid("Members can only view their own fines.");
        }

        try
        {
            // Librarian/Admin can fetch all fines (paid/unpaid). Members only see unpaid.
            bool includePaid = (currentUserRole == "Librarian" || currentUserRole == "Admin");

            var fines = await _service.GetMemberFinesAsync(memberId, includePaid);

            if (!fines.Any())
                return NotFound($"No fines found for member {memberId}.");

            var readDtos = _mapper.Map<IEnumerable<FineReadDto>>(fines);
            return Ok(readDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
        }
    }

    // =========================================================================
    // 2. POST: Pay Fine (Librarian, Admin Action)
    // =========================================================================
    [HttpPost("pay")]
    [Authorize(Roles = "Admin,Librarian")] // Only authorized personnel can register payment
    public async Task<IActionResult> PayFine([FromBody] FinePayDto dto)
    {
        try
        {
            var updatedFine = await _service.PayFineAsync(dto.FineId, dto.PaymentAmount);
            var readDto = _mapper.Map<FineReadDto>(updatedFine);

            return Ok(new
            {
                message = "Fine paid successfully.",
                fine = readDto
            });
        }
        catch (Exception ex)
        {
            // Handle specific errors from the service layer
            if (ex.Message.Contains("not found")) return NotFound(new { error = ex.Message });
            return BadRequest(new { error = ex.Message });
        }
    }
}