using NUnit.Framework;
using OpenQA.Selenium;

namespace TestingBooking
{
    public class BaseTest
    {
        protected IWebDriver driver;

        [SetUp]
        public void TestSetUp()
        {
            driver = DriverFactory.InitDriver();
        }

        [TearDown]
        public void TestTearDown()
        {
            driver.Quit();
        }
    }
}
