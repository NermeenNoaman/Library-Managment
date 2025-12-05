// HRSystem.Infrastructure.Contracts/IMemberService.cs

using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

public interface IMemberService
{
    Task<MEMBER> UpdateMemberProfileAsync(int memberId, MemberUpdateDto dto);
    Task<MEMBER?> GetMemberDetailsAsync(int memberId);
    Task DeleteMemberAsync(int memberId);
}