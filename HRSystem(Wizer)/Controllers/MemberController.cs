// Controllers/MemberController.cs

using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims; 
using System.Threading.Tasks;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class MemberController : ControllerBase
{
    private readonly IMemberService _service;
    private readonly IMemberRepository _repo;
    private readonly IMapper _mapper;

    public MemberController(IMemberService service, IMemberRepository repo, IMapper mapper)
    {
        _service = service;
        _repo = repo;
        _mapper = mapper;
    }

    // -------------------------------------------------------------------------
    // HELPER: Get the current Member ID from the JWT Token
    // -------------------------------------------------------------------------
    private int GetCurrentMemberId()
    {

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            userIdClaim = User.FindFirst("user_id")?.Value;
        }

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
        }

        return userId;
    }

    // =========================================================================
    // 1. GET: Get ALL Members (Admin/Librarian access only)
    // =========================================================================
    [HttpGet]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<IActionResult> GetAllMembers()
    {
        var entities = await _repo.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<MemberReadDto>>(entities);
        return Ok(dtos);
    }

    // =========================================================================
    // 2. GET: Get My Profile (Self-Service) 
    // =========================================================================
    [HttpGet("me")]
    [Authorize(Roles = "Member,Admin,Librarian")]
    public async Task<IActionResult> GetMyProfile()
    {
        int currentUserId = GetCurrentMemberId();

        var member = await _repo.GetMemberByUserIdAsync(currentUserId);

        if (member == null) return NotFound("Member profile not found.");

        var dto = _mapper.Map<MemberReadDto>(member);
        return Ok(dto);
    }

   

    // =========================================================================
    // 4. GET: Get Member by Email (Admin/Librarian access only)
    // =========================================================================
    [HttpGet("email/{email}")]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<IActionResult> GetMemberByEmail(string email)
    {
        var member = await _repo.GetMemberByEmailAsync(email);
        if (member == null) return NotFound($"Member with email {email} not found.");

        var dto = _mapper.Map<MemberReadDto>(member);
        return Ok(dto);
    }

    // =========================================================================
    // 5. PUT: Update Member Profile (Restricted Access)
    // =========================================================================
    [HttpPut("{id}")]
    [Authorize(Roles = "Member")]

    public async Task<IActionResult> UpdateMemberProfile(int id, [FromBody] MemberUpdateDto dto)
    {
        int currentUserId = GetCurrentMemberId();

        
        var memberToUpdate = await _repo.GetByIdAsync(id);

      
        if (!User.IsInRole("Admin") && !User.IsInRole("Librarian"))
        {
            if (memberToUpdate == null || memberToUpdate.user_id != currentUserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Access denied. You can only update your own profile.");
            }

            if (dto.Status != null)
            {
                return BadRequest("Members are not allowed to update their own status.");
            }
        }

        try
        {
            var updatedMember = await _service.UpdateMemberProfileAsync(id, dto);
            var readDto = _mapper.Map<MemberReadDto>(updatedMember);

            return Ok(readDto);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("not found")) return NotFound(new { Error = ex.Message });
            return BadRequest(new { Error = ex.Message });
        }
    }

    // =========================================================================
    // 6. DELETE: Delete Member (Admin/Librarian access only)
    // =========================================================================
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        try
        {
            await _service.DeleteMemberAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            // If service throws error (e.g., Member has active borrowings), handle it here
            return BadRequest(new { Error = ex.Message });
        }
    }
}