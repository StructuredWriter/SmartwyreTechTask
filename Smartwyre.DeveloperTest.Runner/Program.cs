using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Smartwyre.DeveloperTest.Application.Interfaces;
using Smartwyre.DeveloperTest.Application.Services;
using Smartwyre.DeveloperTest.Domain.RequestModels;
using Smartwyre.DeveloperTest.Runner.DI;
using Smartwyre.DeveloperTest.Infrastructure.Persistence;

var provider = ConfigureServices();

// Получаем сервис
var rebateService = provider.GetRequiredService<IRebateService>();

Console.Write("Enter Rebate Identifier: ");
var rebateId = Console.ReadLine();

Console.Write("Enter Product Identifier: ");
var productId = Console.ReadLine();

Console.Write("Enter Volume: ");
var volumeStr = Console.ReadLine();

if (!decimal.TryParse(volumeStr, out var volume))
{
    Console.Write("Cant parse: {0} to decimal", volume);
    return;
}

var request = new CalculateRebateRequestDTO
{
    RebateIdentifier = rebateId,
    ProductIdentifier = productId,
    Volume = volume
};

var result = await rebateService.CalculateAsync(request);

if (result.Success)
    Console.WriteLine("Success. Calculated amount = {Amount}", result.RebateAmount);
else
    Console.WriteLine($"Failed: {result.ErrorMessage}. Check logs");


static ServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    services.AddLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Information);
    });

    services.AddTransient<IRebateDataStore, RebateDataStore>();
    services.AddTransient<IProductDataStore, ProductDataStore>();
    services.AddTransient<IRebateService, RebateService>();

    services.AddRebateCalculators();

    return services.BuildServiceProvider();
}