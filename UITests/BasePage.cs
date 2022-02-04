using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace TestingBooking
{
    public class BasePage
    {
        protected IWebDriver driver;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public bool IsElementVisible(By searchElementBy)
        {
            try
            {
                bool result = driver.FindElement(searchElementBy).Displayed;
                return result;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
