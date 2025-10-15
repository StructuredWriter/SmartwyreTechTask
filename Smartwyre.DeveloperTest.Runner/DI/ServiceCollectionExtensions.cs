using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Application.Services.Calculators;
using Smartwyre.DeveloperTest.Application.Services.Factories.Abstractions;
using Smartwyre.DeveloperTest.Application.Services.Factory;
using Smartwyre.DeveloperTest.Domain.Enums;
using System;

namespace Smartwyre.DeveloperTest.Runner.DI;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers rebate calculators and the factory delegate in the DI container.
    /// </summary>
    public static void AddRebateCalculators(this IServiceCollection services)
    {
        services.AddTransient<IRebateCalculatorFactory, RebateCalculatorFactory>();
        services.AddTransient<FixedCashAmountRebateCalculator>();
        services.AddTransient<FixedRateRebateCalculator>();
        services.AddTransient<AmountPerUomRebateCalculator>();

        services.AddTransient<Func<EIncentiveType, IRebateCalculator>>(provider => type =>
        {
            return type switch
            {
                EIncentiveType.FixedCashAmount => provider.GetRequiredService<FixedCashAmountRebateCalculator>(),
                EIncentiveType.FixedRateRebate => provider.GetRequiredService<FixedRateRebateCalculator>(),
                EIncentiveType.AmountPerUom => provider.GetRequiredService<AmountPerUomRebateCalculator>(),
                _ => throw new NotSupportedException($"Unsupported incentive type: {type}")
            };
        });
    }
}