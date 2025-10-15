using Smartwyre.DeveloperTest.Domain.Enums;

namespace Smartwyre.DeveloperTest.Domain.DTO;

public class RebateCalculationDTO
{
    public int Id { get; set; }
    public string Identifier { get; set; }
    public string RebateIdentifier { get; set; }
    public EIncentiveType IncentiveType { get; set; }
    public decimal Amount { get; set; }
}
