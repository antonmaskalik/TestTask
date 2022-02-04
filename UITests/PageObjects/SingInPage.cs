using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;

namespace TestingBooking.PageObjects
{
    class SingInPage : BasePage
    {
        private By passwordInput = By.XPath("//*[@name='password']");

        [FindsBy(How = How.XPath, Using = "//*[@name='username']")]
        private IWebElement emailAddressInput;

        [FindsBy(How = How.XPath, Using = "//*[contains (@class, '_2__0gVPBP36LBlyHwThlOQ')]")]
        private IWebElement singInBtn;

        public SingInPage(IWebDriver driver) : base(driver) { }

        public void EnterEmailAddress(string emailAddress)
        {
            emailAddressInput.SendKeys(emailAddress);
            singInBtn.Click();
        }

        public void EnterPassword(string password)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(e => IsElementVisible(passwordInput));

            driver.FindElement(passwordInput).SendKeys(password);
            singInBtn.Click();
        }
    }
}
