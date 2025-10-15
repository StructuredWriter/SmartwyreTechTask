using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Infrastructure.Persistence;

public class RebateDataStore : IRebateDataStore
{
    public Task<Rebate> GetRebateAsync(string rebateIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return Task.FromResult(new Rebate());
    }

    public Task StoreCalculationResultAsync(Rebate rebate, decimal rebateAmount)
    {
        if (rebate is null)
            throw new ArgumentNullException(nameof(rebate));

        // Update account in database, code removed for brevity

        return Task.CompletedTask;
    }
}
