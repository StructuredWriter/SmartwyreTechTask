using Microsoft.Extensions.Logging;
using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Application.Services.Factories.Abstractions;
using Smartwyre.DeveloperTest.Domain.DTO;
using Smartwyre.DeveloperTest.Domain.RequestModels;
using System;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Application.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IRebateCalculatorFactory _calculatorFactory;
    private readonly ILogger<RebateService> _logger;

    public RebateService(
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        ILogger<RebateService> logger,
        IRebateCalculatorFactory calculatorFactory)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _logger = logger;
        _calculatorFactory = calculatorFactory;
    }

    /// <summary>
    /// Calculates rebate amount based on provided requestDto.
    /// </summary>
    /// <param name="requestDto">Calculation parameters.</param>
    /// <returns>Result of rebate calculation with success flag and amount.</returns>
    public async Task<CalculateRebateResultDTO> CalculateAsync(CalculateRebateRequestDTO requestDto)
    {
        if (requestDto is null)
            throw new ArgumentNullException(nameof(requestDto));

        _logger.LogInformation("Starting rebate calculation. {@Request}", requestDto);

        try
        {
            var rebate = await _rebateDataStore.GetRebateAsync(requestDto.RebateIdentifier);
            if (rebate is null)
                return Fail("Rebate not found", requestDto.RebateIdentifier);

            var product = await _productDataStore.GetProductAsync(requestDto.ProductIdentifier);
            if (product is null)
                return Fail("Product not found", requestDto.ProductIdentifier);

            _logger.LogDebug("Creating calculator for incentive type: {IncentiveType}", rebate.Incentive);
            var calculator = _calculatorFactory.Create(rebate.Incentive);

            if (!calculator.CanCalculate(rebate, product, requestDto))
            {
                _logger.LogWarning(
                    "Rebate is not applicable for ProductId = {ProductId}, RebateId = {RebateId}, Incentive = {IncentiveType}",
                    requestDto.ProductIdentifier, requestDto.RebateIdentifier, rebate.Incentive);
                return CalculateRebateResultDTO.Failed("Rebate not applicable");
            }

            var rebateAmount = calculator.Calculate(rebate, product, requestDto);
            await _rebateDataStore.StoreCalculationResultAsync(rebate, rebateAmount);

            _logger.LogInformation(
                "Rebate successfully calculated. RebateId = {RebateId}, ProductId = {ProductId}, Amount = {Amount}",
                requestDto.RebateIdentifier, requestDto.ProductIdentifier, rebateAmount);

            return CalculateRebateResultDTO.SuccessResult(rebateAmount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error during rebate calculation. RebateId = {RebateId}, ProductId = {ProductId}",
                requestDto.RebateIdentifier, requestDto.ProductIdentifier);
            return CalculateRebateResultDTO.Failed("Internal error occurred");
        }
        finally
        {
            _logger.LogDebug("Rebate calculation completed for RebateId = {RebateId}", requestDto.RebateIdentifier);
        }
    }

    private CalculateRebateResultDTO Fail(string message, string id)
    {
        _logger.LogWarning("{Message}. Id={Id}", message, id);
        return CalculateRebateResultDTO.Failed(message);
    }
}