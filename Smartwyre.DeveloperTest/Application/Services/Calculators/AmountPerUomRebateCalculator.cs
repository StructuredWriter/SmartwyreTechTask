using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTest.Domain.RequestModels;

namespace Smartwyre.DeveloperTest.Application.Services.Calculators;

public class AmountPerUomRebateCalculator : IRebateCalculator
{
    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequestDTO requestDto)
        => rebate != null && product != null
                          && product.SupportedIncentives.HasFlag(ESupportedIncentiveType.AmountPerUom)
                          && rebate.Amount != 0 && requestDto.Volume != 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequestDTO requestDto)
        => rebate.Amount * requestDto.Volume;
}
