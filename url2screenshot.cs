using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

class Program
{
    static void Main()
    {
        Console.WriteLine("Url to Screenshot tool");

        // Demander à l'utilisateur de saisir l'URL
        Console.Write("Please enter the URL to capture (must be typed like 'https://www.example.com'): ");
        string urlToCapture = Console.ReadLine();

        // Configuration pour exécuter Chrome en mode headless
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless");
        chromeOptions.AddArgument("--disable-gpu");
        chromeOptions.AddArgument("--disable-logging");
        chromeOptions.AddArgument("--blink-settings=imagesEnabled=true");

        // Utilisation de WebDriverManager pour gérer le pilote Chrome
        new DriverManager().SetUpDriver(new ChromeConfig());

        using (var driver = new ChromeDriver(chromeOptions))
        {
            // Accéder à l'URL spécifié
            driver.Navigate().GoToUrl(urlToCapture);

            // Attendre que la page soit complètement chargée (ajustez si nécessaire)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            // Définir la taille de la fenêtre du navigateur à la résolution souhaitée (1080x1920)
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

            // Obtenez le chemin du répertoire contenant l'exécutable
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Créez le chemin complet du fichier de capture d'écran
            string screenshotPath = Path.Combine(exeDirectory, "url_screenshot.png");

            // Prendre une capture d'écran et enregistrer dans le dossier souhaité
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

            Console.WriteLine(" ");
            Console.WriteLine($"Screenshot saved at: {screenshotPath}");
            Console.WriteLine(" ");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            
        }

        Console.WriteLine("Program finished.");
    }
}
