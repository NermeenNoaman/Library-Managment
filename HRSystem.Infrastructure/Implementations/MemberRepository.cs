// HRSystem.Infrastructure.Implementations/MemberRepository.cs

using HRSystem.BaseLibrary.Models;
using HRSystem.BaseLibrary.DTOs;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using HRSystem.Infrastructure.Implementations;
using HRSystem.Infrastructure.Data;

public class MemberRepository : GenericRepository<MEMBER>, IMemberRepository
{
    private readonly HRSystemContext _context;


    // Note: Assuming ApplicationDbContext is your main DbContext
    public MemberRepository(HRSystemContext context) : base(context)
    {
        // 💡 You might need the '!' here if you get the CS8618 warning:
        _context = context; // or _context = context!;
    }

    public async Task<int?> GetMemberIdByUserIdAsync(int userId)
    {
        return await _context.MEMBERs
                          .Where(m => m.user_id == userId)
                          .Select(m => (int?)m.member_id) 
                          .FirstOrDefaultAsync();
    }

    // =======================================================
    // 1. Create Member and User (Transaction)
    // =======================================================
    public async Task<USER> CreateMemberWithUserAsync(MemberCreateDto dto, string passwordHash)
    {
        // 1. Create USER
        var user = new USER
        {
            email = dto.Email,
            password = passwordHash,
            phone = dto.Phone,
            // Assuming USER has a full name field
            // fullname = $"{dto.FirstName} {dto.LastName}", 
            role = "Member",
            created_at = DateTime.UtcNow
        };

        await _context.USERs.AddAsync(user);
        await _context.SaveChangesAsync();

        // 2. Create MEMBER (linked to USER)
        var member = new MEMBER
        {
            user_id = user.user_id,
            email = dto.Email,
            first_name = dto.FirstName,
            last_name = dto.LastName,
            phone = dto.Phone,
            address = dto.Address,
            date_of_birth = dto.DateOfBirth,
            membership_type = dto.MembershipType,
            registration_date = DateTime.UtcNow, // Set registration date upon creation
            status = "Active",
            created_at = DateTime.UtcNow
        };

        await _context.MEMBERs.AddAsync(member);
        await _context.SaveChangesAsync();

        return user;
    }

    // =======================================================
    // 2. Get Member by User ID
    // =======================================================
    public async Task<MEMBER?> GetMemberByUserIdAsync(int userId)
    {
        return await _context.MEMBERs
            .Include(m => m.user)
            .FirstOrDefaultAsync(m => m.user_id == userId);
    }

    // =======================================================
    // 3. Get Member by Email
    // =======================================================
    public async Task<MEMBER?> GetMemberByEmailAsync(string email)
    {
        return await _context.MEMBERs.FirstOrDefaultAsync(m => m.email == email);
    }
}