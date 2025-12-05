// HRSystem.Infrastructure.Contracts/IMemberRepository.cs

using System.Threading.Tasks;
using HRSystem.BaseLibrary.Models;
using System.Collections.Generic;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.Infrastructure.Contracts;

public interface IMemberRepository : IGenericRepository<MEMBER>
{
    Task<USER> CreateMemberWithUserAsync(MemberCreateDto dto, string passwordHash);
    Task<MEMBER?> GetMemberByUserIdAsync(int userId);
    Task<MEMBER?> GetMemberByEmailAsync(string email);
    Task<int?> GetMemberIdByUserIdAsync(int userId);
}