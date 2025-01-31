using IMDbRPA.Models;
using IMDbRPA.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using System.IO;

var builder = Host.CreateApplicationBuilder(args);

// Configurações
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.Configure<Settings>(configuration.GetSection("ImdbRpa"));

// Registrar serviços
builder.Services.AddTransient<IWebDriver>(provider =>
{
    var options = provider.GetRequiredService<IOptions<Settings>>().Value.BrowserOptions;
    return WebDriverFactory.CreateWebDriver(options);
});

builder.Services.AddTransient<IImdbRpaService, ImdbRpaService>();
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
});

using var host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var rpaService = host.Services.GetRequiredService<IImdbRpaService>();

try
{
    logger.LogInformation("Iniciando o processo de RPA...");
    rpaService.LoginAndNavigateToRatings();
    logger.LogInformation("Processo de RPA concluído com sucesso.");
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Erro fatal durante a execução do RPA.");
}