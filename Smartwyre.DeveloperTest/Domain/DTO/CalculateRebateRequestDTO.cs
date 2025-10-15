namespace Smartwyre.DeveloperTest.Domain.RequestModels;

public class CalculateRebateRequestDTO
{
    public string RebateIdentifier { get; set; }

    public string ProductIdentifier { get; set; }

    public decimal Volume { get; set; }
}
