// HRSystem.Infrastructure.Implementations.Services/MemberService.cs

using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using HRSystem.Infrastructure.Data;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repo;
    private readonly HRSystemContext _context;

    public MemberService(IMemberRepository repo, HRSystemContext context)
    {
        _repo = repo;
        _context = context;
    }


    // =======================================================
    // 2. Update Member Profile
    // =======================================================
    public async Task<MEMBER> UpdateMemberProfileAsync(int memberId, MemberUpdateDto dto)
    {
        var member = await _repo.GetByIdAsync(memberId);
        if (member == null)
        {
            throw new Exception("Member not found.");
        }

        // Apply updates to MEMBER entity
        if (dto.FirstName != null) member.first_name = dto.FirstName;
        if (dto.LastName != null) member.last_name = dto.LastName;
        if (dto.Phone != null) member.phone = dto.Phone;
        if (dto.Address != null) member.address = dto.Address;
        if (dto.MembershipType != null) member.membership_type = dto.MembershipType;
        if (dto.Status != null) member.status = dto.Status; // Restricted access needed for status update

        member.updated_at = DateTime.UtcNow;

        // Update linked USER entity if necessary (Email/Phone)
        if (dto.Email != null || dto.Phone != null)
        {
            var user = await _context.USERs.FindAsync(member.user_id);
            if (user != null)
            {
                if (dto.Email != null) user.email = dto.Email;
                if (dto.Phone != null) user.phone = dto.Phone;
                _context.USERs.Update(user);
            }
        }

        await _repo.UpdateAsync(member);
        await _repo.SaveChangesAsync();

        return member;
    }

    // =======================================================
    // 3. Get Member Details
    // =======================================================
    public async Task<MEMBER?> GetMemberDetailsAsync(int memberId)
    {
        return await _repo.GetByIdAsync(memberId);
    }

    // =======================================================
    // 4. Delete Member (Requires cascading delete of USER)
    // =======================================================
    public async Task DeleteMemberAsync(int memberId)
    {
        var member = await _repo.GetByIdAsync(memberId);
        if (member == null) return;

        // Find and delete the linked USER account
        var user = await _context.USERs.FindAsync(member.user_id);

        if (user != null)
        {
            // Note: EF Core might handle the cascading delete of MEMBER if configured, 
            // but we ensure the USER is deleted as well.
            _context.USERs.Remove(user);
        }

        _context.MEMBERs.Remove(member);
        await _context.SaveChangesAsync();
    }
}