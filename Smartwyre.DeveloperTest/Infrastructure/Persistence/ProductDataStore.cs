using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Domain.Entities;

namespace Smartwyre.DeveloperTest.Infrastructure.Persistence;

public class ProductDataStore : IProductDataStore
{
    public Task<Product> GetProductAsync(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return Task.FromResult(new Product());
    }
}
