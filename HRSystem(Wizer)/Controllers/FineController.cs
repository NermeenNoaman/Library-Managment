// Controllers/FineController.cs

using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FineController : ControllerBase
{
    private readonly IFineService _service;
    private readonly IMapper _mapper;

    public FineController(IFineService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    // =========================================================================
    // GET: Fines by Member ID (محمي بالصلاحيات)
    // =========================================================================
    [HttpGet("member/{memberId}")]
    public async Task<IActionResult> GetFinesForMember(int memberId)
    {
        // جلب الدور والـ ID من التوكن
        var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); 

        if (currentUserIdClaim == null || !int.TryParse(currentUserIdClaim.Value, out int currentUserId))
        {
             return Unauthorized("Invalid user ID in token.");
        }

        // 🛡️ RBAC: إذا كان المستخدم عضواً، يجب أن يطلب غراماته فقط
        if (currentUserRole == "Member" && currentUserId != memberId)
        {
            return Forbid("Members can only view their own fines.");
        }

        try
        {
            // Librarian/Admin يستطيع جلب جميع الغرامات (includePaid = true)
            bool includePaid = (currentUserRole == "Librarian" || currentUserRole == "Admin");

            var fines = await _service.GetMemberFinesAsync(memberId, includePaid);
            
            if (fines == null || !fines.Any())
                return NotFound($"No fines found for member {memberId}.");

            var readDtos = _mapper.Map<IEnumerable<FineReadDto>>(fines);
            return Ok(readDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // =========================================================================
    // POST: Pay Fine (للمكتبيين والمدراء فقط)
    // =========================================================================
    [HttpPost("pay")]
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
            // 404 إذا لم يتم العثور على الغرامة، 400 إذا كان المبلغ غير كافٍ
            if (ex.Message.Contains("not found")) return NotFound(new { error = ex.Message });
            return BadRequest(new { error = ex.Message });
        }
    }
}