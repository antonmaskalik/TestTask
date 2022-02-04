using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace TestingBooking.PageObjects
{
    class SearchResultsPage : BasePage
    {
        IEnumerable<IWebElement> allAddresses;
        IEnumerable<IWebElement> allNightsAndPeopleFilters;

        private By addresses = By.XPath("//*[@data-testid='address']");
        private By nightsAndPeopleFilters = By.XPath("//*[@data-testid='price-for-x-nights']");

        public SearchResultsPage(IWebDriver driver) : base(driver) { }

        public bool IsCityFilterApplied(string city)
        {
            allAddresses = driver.FindElements(addresses);

            return allAddresses.All(e => e.Text.Contains(city));
        }

        public bool IsCountNihgtsFilterApplied(uint nights)
        {
            allNightsAndPeopleFilters = driver.FindElements(nightsAndPeopleFilters);

            return allNightsAndPeopleFilters.All(e => e.Text.Contains(nights.ToString() + " night"));
        }

        public bool IsCountAdultsFilterApplied(uint adults)
        {
            allNightsAndPeopleFilters = driver.FindElements(nightsAndPeopleFilters);

            return allNightsAndPeopleFilters.All(e => e.Text.Contains(adults.ToString() + " adults"));
        }

        public bool IsCountChildrenFilterApplied(uint children)
        {
            allNightsAndPeopleFilters = driver.FindElements(nightsAndPeopleFilters);

            if (children != 0)
            {
                return allNightsAndPeopleFilters.All(e => e.Text.Contains(children.ToString() + " child"));
            }
            else
            {
                return true;
            }
        }
    }
}
