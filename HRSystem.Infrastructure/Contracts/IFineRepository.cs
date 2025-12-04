// HRSystem.Infrastructure.Contracts/IFineRepository.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;

public interface IFineRepository : IGenericRepository<FINE> 
{
    Task<IEnumerable<FINE>> GetUnpaidFinesByMemberIdAsync(int memberId);
    Task<FINE> GetFineByIdAsync(int fineId);
}