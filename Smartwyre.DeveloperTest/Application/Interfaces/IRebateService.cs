using System.Threading.Tasks;
using Smartwyre.DeveloperTest.Domain.DTO;
using Smartwyre.DeveloperTest.Domain.RequestModels;

namespace Smartwyre.DeveloperTest.Application.Interfaces;

public interface IRebateService
{
    Task<CalculateRebateResultDTO> CalculateAsync(CalculateRebateRequestDTO requestDto);
}
