using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.RequestModels;

namespace Smartwyre.DeveloperTest.Application.Interfaces;

public interface IRebateCalculator
{
    bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequestDTO requestDto);
    decimal Calculate(Rebate rebate, Product product, CalculateRebateRequestDTO requestDto);
}
