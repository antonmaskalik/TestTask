using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingBooking.PageObjects
{
    public class HeaderPage : BasePage
    {
        const string URL = "https://www.booking.com/";

        private string dateSelectionFormat = "//*[@data-date='{0}']";
        private string dateFormat = "yyyy-MM-dd";

        IEnumerable<IWebElement> allCurrencies;
        IEnumerable<IWebElement> allLanguages;

        private By currencies = By.ClassName("bui-traveller-header__currency");
        private By languages = By.ClassName("bui-inline-container__main");
        private By currentLanguage = By.XPath("//*[@class='bk-icon -streamline-checkmark']/ancestor::div/div[@class='bui-inline-container__main']");
        private By ageChildrenSelect = By.XPath("//select[@name='age']");

        [FindsBy(How = How.XPath, Using = "//*[@class='bui-button__text']/span[@aria-hidden]")]
        private IWebElement currencyBtn;

        [FindsBy(How = How.ClassName, Using = "bui-avatar__image")]
        private IWebElement languageBtn;

        [FindsBy(How = How.XPath, Using = "//*[@data-decider-header = 'flights']")]
        private IWebElement flightsBtn;

        [FindsBy(How = How.XPath, Using = "//*[@data-et-click='customGoal:YTBUIHOdBOcaGPaVHXT:2']")]
        private IWebElement signInBtn;

        [FindsBy(How = How.Id, Using = "profile-menu-trigger--title")]
        private IWebElement yourAccountBtn;

        [FindsBy(How = How.Id, Using = "ss")]
        private IWebElement enterCityInput;

        [FindsBy(How = How.ClassName, Using = "xp__dates-inner")]
        private IWebElement chooseDateBtn;

        [FindsBy(How = How.Id, Using = "xp__guests__toggle")]
        private IWebElement chooseGuestsBtn;

        [FindsBy(How = How.XPath, Using = "(//*[@class='bui-stepper__display'])[1]")]
        private IWebElement adultsCount;

        [FindsBy(How = How.XPath, Using = "(//*[@class='bui-stepper__display'])[2]")]
        private IWebElement childrenCount;

        [FindsBy(How = How.XPath, Using = "(//*[@class='bui-stepper__display'])[3]")]
        private IWebElement roomsCount;

        [FindsBy(How = How.XPath, Using = "(//*[@data-bui-ref='input-stepper-subtract-button'])[1]")]
        private IWebElement subtractAdultsBtn;

        [FindsBy(How = How.XPath, Using = "(//*[@data-bui-ref='input-stepper-add-button'])[1]")]
        private IWebElement addAdultsBtn;

        [FindsBy(How = How.XPath, Using = "(//*[@data-bui-ref='input-stepper-subtract-button'])[2]")]
        private IWebElement subtractChildrenBtn;

        [FindsBy(How = How.XPath, Using = "(//*[@data-bui-ref='input-stepper-add-button'])[2]")]
        private IWebElement addChildrenBtn;

        [FindsBy(How = How.XPath, Using = "(//*[@data-bui-ref='input-stepper-subtract-button'])[3]")]
        private IWebElement subtractRoomsBtn;

        [FindsBy(How = How.XPath, Using = "(//*[@data-bui-ref='input-stepper-add-button'])[3]")]
        private IWebElement addRoomsBtn;

        [FindsBy(How = How.ClassName, Using = "sb-searchbox__button")]
        private IWebElement searchBtn;


        public HeaderPage(IWebDriver driver) : base(driver) { }

        public void GoToUrl()
        {
            driver.Navigate().GoToUrl(URL);
        }

        public void ChooseCurrency(string currency)
        {
            currencyBtn.Click();
            allCurrencies = driver.FindElements(currencies);
            allCurrencies.First(c => c.Text.Contains(currency)).Click();
        }

        public string GetCurrentCurrency()
        {
            return currencyBtn.Text;
        }

        public void ChooseLanguage(string language)
        {
            languageBtn.Click();
            allLanguages = driver.FindElements(languages);
            allLanguages.First(l => l.Text.Contains(language)).Click();
        }

        public string GetCurrentLanguage()
        {
            languageBtn.Click();
            return driver.FindElement(currentLanguage).Text;
        }

        public void GoToFlightsPage()
        {
            flightsBtn.Click();
        }

        public void ClickSingIn()
        {
            signInBtn.Click();
        }

        public bool IsYourAccountDisplayed()
        {
            return yourAccountBtn.Displayed;
        }

        public void EnterCity(string city)
        {
            enterCityInput.SendKeys(city);
        }

        public void ChooseDateOfStayFilter(DateTime checkInDate, DateTime checkOutDate)
        {
            chooseDateBtn.Click();

            driver.FindElement(By.XPath(string.Format(dateSelectionFormat, checkInDate.ToString(dateFormat)))).Click();
            driver.FindElement(By.XPath(string.Format(dateSelectionFormat, checkOutDate.ToString(dateFormat)))).Click();
        }

        public void SetAdultsCount(uint adults)
        {
            chooseGuestsBtn.Click();

            uint adultsCount = uint.Parse(this.adultsCount.Text);

            while (adultsCount != adults)
            {
                if (adultsCount < adults)
                {
                    addAdultsBtn.Click();
                    adultsCount++;
                }
                else
                {
                    subtractAdultsBtn.Click();
                    adultsCount--;
                }
            }
        }

        public void SetChildrenCount(uint children, string[] ageChildren)
        {
            chooseGuestsBtn.Click();

            uint childrenCount = uint.Parse(this.childrenCount.Text);

            while (childrenCount != children)
            {
                if (childrenCount < children)
                {
                    addChildrenBtn.Click();
                    childrenCount++;
                }
                else
                {
                    subtractChildrenBtn.Click();
                    childrenCount--;
                }
            }

            if (children > 0)
            {
                int i = 0;

                IEnumerable<IWebElement> allAgeChildrenSelects = driver.FindElements(ageChildrenSelect);

                foreach (var e in allAgeChildrenSelects)
                {    
                    SelectElement ageSelect = new SelectElement(e);

                    while(i < ageChildren.Length)
                    {
                        ageSelect.SelectByValue(ageChildren[i]);
                        i++;
                        break;
                    }                    
                }
            }
        }

        public void SetRoomsCount(uint rooms)
        {
            chooseGuestsBtn.Click();

            uint roomsCnt = uint.Parse(roomsCount.Text);

            while (roomsCnt != rooms)
            {
                if (roomsCnt < rooms)
                {
                    addRoomsBtn.Click();
                    roomsCnt++;
                }
                else
                {
                    subtractRoomsBtn.Click();
                    roomsCnt--;
                }
            }
        }

        public void ClickSearchBtn()
        {
            searchBtn.Click();
        }
    }
}
