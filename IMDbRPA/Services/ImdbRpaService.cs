using IMDbRPA.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace IMDbRPA.Services;
public class ImdbRpaService : IImdbRpaService
{
    private readonly IWebDriver _driver;
    private readonly Settings _settings;
    private readonly ILogger<ImdbRpaService> _logger;

    public ImdbRpaService(IWebDriver driver, IOptions<Settings> settings, ILogger<ImdbRpaService> logger)
    {
        _driver = driver;
        _settings = settings.Value;
        _logger = logger;
    }

    public void LoginAndNavigateToRatings()
    {
        try
        {
            _logger.LogInformation("Navegando para a página de login...");
            _driver.Navigate().GoToUrl(_settings.LoginUrl);


            if (string.IsNullOrEmpty(_settings.Credentials.Email) || string.IsNullOrEmpty(_settings.Credentials.Password))
            {
                _logger.LogError("É necessário informar usuário e senha!");
                return;
            }

            // Preenche o email
            var emailField = WaitForElement(By.Id("ap_email"));
            RandomDelay();
            emailField.Clear();
            emailField.SendKeys(_settings.Credentials.Email);

            // Preenche a senha
            var passwordField = WaitForElement(By.Id("ap_password"));
            RandomDelay();
            passwordField.Clear();
            passwordField.SendKeys(_settings.Credentials.Password);

            var loginButton = WaitForElement(By.Id("signInSubmit"));
            RandomDelay();
            loginButton.Click();

            var pageSource = _driver.PageSource;
            if (pageSource.Contains("Your password is incorrect"))
            {
                Console.WriteLine("Senha informada esta errada!");
                return;
            }
            else if (pageSource.Contains("Solve this puzzle to protect your account"))
            {
                _logger.LogError("Pagina esta pedindo para quebrar captcha!");
                return;
            }

            _logger.LogInformation("Login realizado com sucesso.");

            WaitForElement(By.CssSelector(".ipc-icon:nth-child(3)"));

            var accountMenuIcon = WaitForElement(By.CssSelector(".ipc-icon:nth-child(3)"));
            RandomDelay();
            accountMenuIcon.Click();

            var ratingsOption = WaitForElement(By.CssSelector(".imdb-header__account-menu > .ipc-list__item:nth-child(5) > .ipc-list-item__text"));
            RandomDelay();
            ratingsOption.Click();

            _logger.LogInformation("Navegação para a página de classificações concluída.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante a execução do RPA.");
            throw;
        }
        finally
        {
            _driver?.Quit();
        }
    }

    private IWebElement WaitForElement(By locator, int timeoutInSeconds = 10)
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
        return wait.Until(driver => driver.FindElement(locator));
    }

    private void RandomDelay()
    {
        var random = new Random();
        int delaySeconds = random.Next(_settings.Delays.MinSeconds, _settings.Delays.MaxSeconds);
        int delayMilliseconds = delaySeconds * 1000; // Converte segundos para milissegundos
        Thread.Sleep(delayMilliseconds);
    }
}
