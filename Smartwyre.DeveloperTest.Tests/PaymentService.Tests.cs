using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Application.Services;
using Smartwyre.DeveloperTest.Application.Services.Factories.Abstractions;
using Smartwyre.DeveloperTest.Domain.Entities;
using Smartwyre.DeveloperTest.Domain.Enums;
using Smartwyre.DeveloperTest.Domain.RequestModels;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private readonly Mock<IRebateDataStore> _rebateDataStore = new();
    private readonly Mock<IProductDataStore> _productDataStore = new();
    private readonly Mock<IRebateCalculatorFactory> _factory = new();
    private readonly Mock<IRebateCalculator> _calculator = new();
    private readonly Mock<ILogger<RebateService>> _logger = new();

    private RebateService CreateService()
    {
        return new RebateService(
            _rebateDataStore.Object,
            _productDataStore.Object,
            _logger.Object,
            _factory.Object
        );
    }

    [Fact]
    public async Task CalculateAsync_ReturnsFailed_WhenRebateNotFound()
    {
        _rebateDataStore.Setup(r => r.GetRebateAsync(It.IsAny<string>())).ReturnsAsync((Rebate)null);
        var service = CreateService();

        var result = await service.CalculateAsync(new CalculateRebateRequestDTO());

        Assert.False(result.Success);
        Assert.Equal("Rebate not found", result.ErrorMessage);
    }

    [Fact]
    public async Task CalculateAsync_ReturnsFailed_WhenProductNotFound()
    {
        _rebateDataStore.Setup(r => r.GetRebateAsync(It.IsAny<string>()))
            .ReturnsAsync(new Rebate { Incentive = EIncentiveType.FixedCashAmount });
        _productDataStore.Setup(p => p.GetProductAsync(It.IsAny<string>()))
            .ReturnsAsync((Product)null);

        var service = CreateService();

        var result = await service.CalculateAsync(new CalculateRebateRequestDTO());

        Assert.False(result.Success);
        Assert.Equal("Product not found", result.ErrorMessage);
    }

    [Fact]
    public async Task CalculateAsync_ReturnsFailed_WhenCannotCalculate()
    {
        // Arrange
        var rebate = new Rebate { Incentive = EIncentiveType.FixedRateRebate };
        var product = new Product();

        _rebateDataStore.Setup(r => r.GetRebateAsync(It.IsAny<string>())).ReturnsAsync(rebate);
        _productDataStore.Setup(p => p.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

        _calculator.Setup(c => c.CanCalculate(rebate, product, It.IsAny<CalculateRebateRequestDTO>()))
            .Returns(false);

        _factory.Setup(f => f.Create(EIncentiveType.FixedRateRebate))
            .Returns(_calculator.Object);

        var service = CreateService();

        var result = await service.CalculateAsync(new CalculateRebateRequestDTO());

        Assert.False(result.Success);
        Assert.Equal("Rebate not applicable", result.ErrorMessage);
    }

    [Fact]
    public async Task CalculateAsync_ReturnsSuccess_WhenCalculationIsValid()
    {
        var rebate = new Rebate { Incentive = EIncentiveType.FixedRateRebate };
        var product = new Product();
        var request = new CalculateRebateRequestDTO { Volume = 10m };

        _rebateDataStore.Setup(r => r.GetRebateAsync(It.IsAny<string>())).ReturnsAsync(rebate);
        _productDataStore.Setup(p => p.GetProductAsync(It.IsAny<string>())).ReturnsAsync(product);

        _calculator.Setup(c => c.CanCalculate(rebate, product, request)).Returns(true);
        _calculator.Setup(c => c.Calculate(rebate, product, request)).Returns(25m);

        _factory.Setup(f => f.Create(EIncentiveType.FixedRateRebate)).Returns(_calculator.Object);

        var service = CreateService();
        var result = await service.CalculateAsync(request);

        Assert.True(result.Success);
        Assert.Equal(25m, result.RebateAmount);
        _rebateDataStore.Verify(r => r.StoreCalculationResultAsync(rebate, 25m), Times.Once);
    }
}