using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Domain.Entities;

namespace Smartwyre.DeveloperTest.Application.Interfaces;

public interface IRebateDataStore
{
    Task<Rebate> GetRebateAsync(string rebateIdentifier);

    Task StoreCalculationResultAsync(Rebate account, decimal rebateAmount);
}
