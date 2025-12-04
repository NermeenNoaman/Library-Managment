// HRSystem.Infrastructure.Contracts/IFineService.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.BaseLibrary.Models;

public interface IFineService
{
    Task<IEnumerable<FINE>> GetMemberFinesAsync(int memberId, bool includePaid = false);
    Task<FINE> PayFineAsync(int fineId, decimal paymentAmount);
}