using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTest.Application.Services.Factories.Abstractions;

public interface IRebateCalculatorFactory
{
    IRebateCalculator Create(EIncentiveType type);
}
