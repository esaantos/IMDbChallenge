using IMDbRPA.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

public static class WebDriverFactory
{
    public static IWebDriver CreateWebDriver(BrowserOptions options)
    {
        var chromeOptions = new ChromeOptions();

        // 1. Configurações Básicas de Stealth
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-gpu");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        chromeOptions.AddArgument("--disable-web-security");
        chromeOptions.AddArgument("--allow-running-insecure-content");

        // 2. Desabilitar Automação Detectável
        chromeOptions.AddExcludedArgument("enable-automation"); // Remove a mensagem "navegador controlado por automação"
        chromeOptions.AddAdditionalOption("useAutomationExtension", false);

        // 3. Spoof de User-Agent Realista
        chromeOptions.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");

        // 4. Configurações Avançadas de Fingerprinting
        chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
        chromeOptions.AddArgument("--disable-features=IsolateOrigins,site-per-process");
        chromeOptions.AddArgument("--disable-ipc-flooding-protection");


        // 6. Perfil de Navegador Personalizado
        chromeOptions.AddArgument("--user-data-dir=/path/to/custom/profile"); // Reutilizar perfil existente
        chromeOptions.AddArgument("--profile-directory=Default");

        // 7. Desabilitar WebRTC e WebGL
        chromeOptions.AddArgument("--disable-webrtc");
        chromeOptions.AddArgument("--disable-webgl");

        // 8. Configurar Hardware Concurrency Falso
        chromeOptions.AddArgument("--disable-device-discovery-notifications");
        chromeOptions.AddArgument("--use-fake-ui-for-media-stream");

        var driver = new ChromeDriver(chromeOptions);
        return driver;
    }
}

