// HRSystem_Wizer_.Controllers/ReservationController.cs

using AutoMapper;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize] // Base authentication required for all actions
public class ReservationController : ControllerBase
{
    private readonly IReservationService _service;
    private readonly IReservationRepository _repo;
    private readonly IUserRepository _userRepo;
    private readonly IMemberRepository _memberRepo; 
    private readonly IMapper _mapper;

    public ReservationController(IReservationService service, IReservationRepository repo, IUserRepository userRepo, IMapper mapper, IMemberRepository memberRepo)
    {
        _service = service;
        _repo = repo;
        _userRepo = userRepo;
        _mapper = mapper;
        _memberRepo = memberRepo;
    }

    // -------------------------------------------------------------------------
    // HELPER: Get the current Member ID from the JWT Token (Assumed to be here or BaseApiController)
    // -------------------------------------------------------------------------
    private int GetCurrentUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("User ID claim is missing or invalid in the token.");
        }
        return userId;
    }

    // =========================================================================
    // 1. POST: Reserve a Book (Member Action)
    // =========================================================================
    [HttpPost("reserve/{bookId}")]
    [Authorize(Roles = "Member")] // Only members can reserve
    public async Task<IActionResult> ReserveBook(int bookId)
    {
        try
        {
            int userId = GetCurrentUserIdFromToken();

            int? memberId = await _memberRepo.GetMemberIdByUserIdAsync(userId);

            if (memberId == null || memberId.Value == 0)
            {
                return BadRequest(new { Error = "User account is not associated with an active Member profile." });
            }

            
            var resultDto = await _service.CreateReservationAsync(memberId.Value, bookId);

            return CreatedAtAction(nameof(GetMyReservations), null, resultDto);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Authentication failed or token is invalid.");
        }
        catch (InvalidOperationException ex)
        {
            // Handles 'Book is available' or 'Already reserved' errors
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "An error occurred during reservation: " + ex.Message });
        }
    }

    // =========================================================================
    // 2. GET: Get ALL Reservations (Admin, Librarian)
    // =========================================================================
    [HttpGet]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<IActionResult> GetAll()
    {
        var entities = await _repo.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<ReservationReadDto>>(entities);
        return Ok(dtos);
    }

    // =========================================================================
    // 3. GET: Get Reservations by Member Email (Admin, Librarian)
    // =========================================================================
    [HttpGet("member-email/{email}")]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<IActionResult> GetReservationsByEmail(string email)
    {
        // 1. Find User to get their ID
        var user = await _userRepo.GetByUsernameAsync(email);
        if (user == null)
        {
            return NotFound($"User account with email {email} not found.");
        }

        // 2. Find Reservations using the User ID
        // Note: This relies on the convention that User ID is the identifier for the member in GetMyReservationsAsync
        var reservations = await _service.GetMyReservationsAsync(user.user_id);

        if (!reservations.Any())
        {
            return NotFound($"No reservations found for member with email {email}.");
        }

        return Ok(reservations);
    }

    // =========================================================================
    // 4. GET: Get My Active Reservations (Self-Service)
    // =========================================================================
    [HttpGet("my")]
    [Authorize(Roles = "Member")]
    public async Task<IActionResult> GetMyReservations()
    {
        int userId = GetCurrentUserIdFromToken();

        int? memberId = await _memberRepo.GetMemberIdByUserIdAsync(userId);

        if (memberId == null || memberId.Value == 0)
        {
            return NotFound("Member profile not found or not fully registered.");
        }

        var reservations = await _service.GetMyReservationsAsync(memberId.Value);

        if (!reservations.Any())
        {
            return NotFound("No active reservations found for the current member.");
        }

        return Ok(reservations);
    }

    // =========================================================================
    // 5. PUT: Update Reservation Status (Admin, Librarian) - REQUESTED ENDPOINT
    // =========================================================================
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Librarian")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReservationReadDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateReservation(int id, [FromBody] ReservationUpdateDto dto)
    {
        try
        {
            // The service handles status change, validation, and returns the updated DTO
            var result = await _service.UpdateReservationStatusAsync(id, dto);

            if (result == null)
                return NotFound($"Reservation with ID {id} not found.");

            return Ok(result); // Returns 200 OK with the updated record
        }
        catch (InvalidOperationException ex)
        {
            // Catches business rules exceptions (e.g., trying to fulfill an already cancelled reservation)
            return BadRequest(new { Error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Error = "An error occurred during status update: " + ex.Message });
        }
    }





    // =========================================================================
    // 6. DELETE: Cancel Reservation (Admin, Librarian)
    // =========================================================================
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Librarian")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return NotFound();

        // Use service if complex logic (like notifying the next user) is needed
        await _repo.DeleteAsync(entity);
        await _repo.SaveChangesAsync();

        return NoContent();
    }
}