using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace TestingBooking
{
    public static class DriverFactory
    {
        static IWebDriver driver;

        public static IWebDriver InitDriver()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(11);

            return driver;
        }
    }
}