using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace TestingBooking
{
    public static class DriverFactory
    {
        static IWebDriver driver = null;

        public static IWebDriver InitDriver()
        {
            if(driver == null)
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(11);
            }

            return driver;
        }
    }
}