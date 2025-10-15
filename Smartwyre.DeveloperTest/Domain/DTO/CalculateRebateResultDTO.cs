namespace Smartwyre.DeveloperTest.Domain.DTO;

public record CalculateRebateResultDTO(bool Success, decimal? RebateAmount = null, string ErrorMessage = null)
{
    public static CalculateRebateResultDTO Failed(string message)
        => new(false, null, message);

    public static CalculateRebateResultDTO SuccessResult(decimal amount)
        => new(true, amount);
}