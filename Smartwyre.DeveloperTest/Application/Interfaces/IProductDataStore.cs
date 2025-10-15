using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Domain.Entities;

namespace Smartwyre.DeveloperTest.Application.Interfaces;

public interface IProductDataStore
{
    Task<Product> GetProductAsync(string productIdentifier);
}
