using NUnit.Framework;
using TestingBooking.PageObjects;
using System;

namespace TestingBooking
{
    [TestFixture]
    class BookingTests : BaseTest
    {
        [Test]
        public void ChangeСurrencyTest()
        {
            string currency = "USD";

            HeaderPage headerPage = new HeaderPage(driver);
            headerPage.GoToUrl();
            headerPage.ChooseCurrency(currency);

            Assert.AreEqual(currency, headerPage.GetCurrentCurrency(), "The currency should be changed to {0}", currency);
        }

        [Test]
        public void ChangeLanguageTest()
        {
            string language = "English (US)";

            HeaderPage headerPage = new HeaderPage(driver);
            headerPage.GoToUrl();
            headerPage.ChooseLanguage(language);

            Assert.AreEqual(language, headerPage.GetCurrentLanguage(), "The language should be changed to {0}", language);
        }

        [Test]
        public void GoToFlightsPageTest()
        {
            HeaderPage headerPage = new HeaderPage(driver);
            headerPage.GoToUrl();
            headerPage.GoToFlightsPage();

            FlightsPage flightsPage = new FlightsPage(driver);

            Assert.IsTrue(flightsPage.IsFlightsPageOpened(), "The user must go to the flights page");
        }

        [Test]
        public void SingInTest()
        {
            string emailAddress = "java2.test@yandex.com";
            string password = "xa5-Yzz-bB7-22j";

            HeaderPage headerPage = new HeaderPage(driver);
            headerPage.GoToUrl();
            headerPage.ClickSingIn();

            SingInPage singInPage = new SingInPage(driver);
            singInPage.EnterEmailAddress(emailAddress);
            singInPage.EnterPassword(password);

            Assert.IsTrue(headerPage.IsYourAccountDisplayed(), "The user must sign in his account");
        }

        [Test]
        public void FilterTest()
        {
            string language = "English (US)";
            string city = "Minsk";
            uint adults = 3;
            uint children = 1;
            string[] ageChildren = new string[] { "7" };
            uint rooms = 1;

            DateTime checkInDate = DateTime.Now.AddDays(7);
            DateTime checkOutDate = checkInDate.AddDays(2);

            uint nights = uint.Parse((checkOutDate - checkInDate).ToString("dd"));

            HeaderPage headerPage = new HeaderPage(driver);
            headerPage.GoToUrl();
            headerPage.ChooseLanguage(language);
            headerPage.EnterCity(city);
            headerPage.ChooseDateOfStayFilter(checkInDate, checkOutDate);
            headerPage.SetAdultsCount(adults);
            headerPage.SetChildrenCount(children, ageChildren);
            headerPage.SetRoomsCount(rooms);

            headerPage.ClickSearchBtn();

            SearchResultsPage searchResultsPage = new SearchResultsPage(driver);

            Assert.IsTrue(searchResultsPage.IsCityFilterApplied(city), "All results should be contain city {0}", city);
            Assert.IsTrue(searchResultsPage.IsCountAdultsFilterApplied(adults), "All results should be for {0} adults", adults);
            Assert.IsTrue(searchResultsPage.IsCountChildrenFilterApplied(children), "All results should be for {0} children", children);
            Assert.IsTrue(searchResultsPage.IsCountNihgtsFilterApplied(nights), "All results should be booked for {0} nights", nights);
        }
    }
}
