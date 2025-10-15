using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Application.Services.Factories.Abstractions;
using Smartwyre.DeveloperTest.Domain.Enums;
using System;

namespace Smartwyre.DeveloperTest.Application.Services.Factory;

public class RebateCalculatorFactory : IRebateCalculatorFactory
{
    private readonly Func<EIncentiveType, IRebateCalculator> _factoryDelegate;

    public RebateCalculatorFactory(Func<EIncentiveType, IRebateCalculator> factoryDelegate)
    {
        _factoryDelegate = factoryDelegate;
    }

    public IRebateCalculator Create(EIncentiveType type)
    {
        return _factoryDelegate(type);
    }
}
