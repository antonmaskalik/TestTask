using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace TestingBooking.PageObjects
{
    class FlightsPage : BasePage
    {
        [FindsBy(How = How.ClassName, Using = "search-form-inner")]
        private IWebElement searchForm;

        public FlightsPage(IWebDriver driver) : base(driver) { }

        public bool IsFlightsPageOpened()
        {
            return searchForm.Displayed;
        }
    }
}
