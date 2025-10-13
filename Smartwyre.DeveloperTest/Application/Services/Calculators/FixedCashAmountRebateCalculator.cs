using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTest.Domain.RequestModels;

namespace Smartwyre.DeveloperTest.Application.Services.Calculators;

public class FixedCashAmountRebateCalculator : IRebateCalculator
{
    public bool CanCalculate(Rebate rebate, Product product, CalculateRebateRequestDTO requestDto)
        => rebate?.Incentive == EIncentiveType.FixedCashAmount
           && product?.SupportedIncentives.HasFlag(ESupportedIncentiveType.FixedCashAmount) == true
           && rebate.Amount != 0;

    public decimal Calculate(Rebate rebate, Product product, CalculateRebateRequestDTO requestDto)
        => rebate.Amount;
}
