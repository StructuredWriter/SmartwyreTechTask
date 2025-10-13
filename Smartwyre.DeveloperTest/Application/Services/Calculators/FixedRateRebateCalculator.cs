using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTest.Domain.RequestModels;

namespace Smartwyre.DeveloperTest.Application.Services.Calculators;

public class FixedRateRebateCalculator : IRebateCalculator
{
    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequestDTO requestDto)
        => rebate != null && product != null
                          && product.SupportedIncentives.HasFlag(ESupportedIncentiveType.FixedRateRebate)
                          && rebate.Percentage != 0 && product.Price != 0 && requestDto.Volume != 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequestDTO requestDto)
        => product.Price * rebate.Percentage * requestDto.Volume;
}
