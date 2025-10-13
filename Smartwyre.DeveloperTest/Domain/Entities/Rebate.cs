using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTest.Domain.Entities;

public class Rebate
{
    public string Identifier { get; set; }
    public EIncentiveType Incentive { get; set; }
    public decimal Amount { get; set; }
    public decimal Percentage { get; set; }
}
