using NUnit.Framework;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace ApiTests
{
    [TestFixture]
    public class ApiTests
    {
        readonly StringBuilder baseUrl = new StringBuilder("https://www.metaweather.com/api/location/");
        ResponseHelper responseHelper = new ResponseHelper();

        readonly string dateFormat = "yyyy/MM/d";
        readonly string titlePropertyName = "title";
        readonly string woeidPropertyName = "woeid";
        readonly string weatherStateNamePropertyName = "weather_state_name";
        readonly string minTempPropertyName = "min_temp";
        string woeid = "834463";

        [Test]
        [TestCase("min", "minsk")]
        public void AllCityMathesTest(string textToSearch, string expectedCityName)
        {
            string url = baseUrl + "search/?query=" + textToSearch;

            var requestContent = responseHelper.GetResponseContentAsync(url, HttpMethod.Get);
            var items = responseHelper.GetArrayEnumerator(requestContent.Result);

            Assert.IsTrue(items.All(x => x.GetProperty(titlePropertyName).GetString().ToLower().Contains(textToSearch)), "The Title must contain '{0}'.", textToSearch);
            Assert.AreNotEqual(items.FirstOrDefault(x => x.GetProperty(titlePropertyName).GetString().ToLower() == expectedCityName).ValueKind, JsonValueKind.Undefined, "City '{0}' should exist in the result list.", expectedCityName);
        }

        [Test]
        [TestCase("53.90255,27.563101", "minsk")]
        public void CheckLatLongTest(string curentLatLong, string expectedCityName)
        {
            string url = baseUrl + "search/?lattlong=" + curentLatLong;

            var requestContent = responseHelper.GetResponseContentAsync(url, HttpMethod.Get);
            var items = responseHelper.GetArrayEnumerator(requestContent.Result);

            Assert.IsTrue(items.FirstOrDefault().GetProperty(titlePropertyName).GetString().ToLower() == expectedCityName, "City with given geodata '{0}' must match the expected city name {1}", curentLatLong, expectedCityName);

            //get woeid to use in GetWeatherTest
            woeid = items.FirstOrDefault().GetProperty(woeidPropertyName).ToString();
        }

        [Test]
        public void GetWeatherByGeodataTest()
        {
            DateTime dateToday = DateTime.Now;
            string url = baseUrl + woeid + "/" + dateToday.ToString(dateFormat, CultureInfo.GetCultureInfo("en-US"));

            var requestContent = responseHelper.GetResponseContentAsync(url, HttpMethod.Get);
            var items = responseHelper.GetArrayEnumerator(requestContent.Result);

            Assert.IsTrue(items.All(i => i.GetProperty(weatherStateNamePropertyName).ValueKind != JsonValueKind.Undefined), "Weather information exists for location {0} and date {1}", woeid, dateToday);
        }

        [Test]
        public void CheckTempMatchesSeasonTest()
        {
            DateTime dateToday = DateTime.Now;
            string url = baseUrl + woeid + "/" + dateToday.ToString(dateFormat, CultureInfo.GetCultureInfo("en-US"));

            List<double> minTempProperties = new List<double>();
            var requestContent = responseHelper.GetResponseContentAsync(url, HttpMethod.Get);
            var items = responseHelper.GetArrayEnumerator(requestContent.Result);

            foreach (JsonElement i in items)
            {
                minTempProperties.Add(i.GetProperty(minTempPropertyName).GetDouble());
            }

            //Get the most minimal temperature for day
            minTempProperties.Sort();
            double minTemp = minTempProperties.Last();

            switch (GetSeason(dateToday.Month.ToString()))
            {
                case "winter":
                    {
                        Assert.IsTrue(minTemp <= 0, "In winter the temperature should be < 0");
                        break;
                    }
                case "summer":
                    {
                        Assert.IsTrue(minTemp > 0, "In summer the temperature should be > 0");
                        break;
                    }
                case "spring":
                case "autumn":
                    {
                        Assert.IsTrue(minTemp >= -10 && minTemp <= +20, "In spring and autunm the temperature can be in the range of -10 - +20");
                        break;
                    }
            }
        }

        [Test]
        public void CheckWetherStateNameFiveYearsAgoAndNowTest()
        {
            DateTime dateToday = DateTime.Now;

            string urlNow = baseUrl + woeid + "/" + dateToday.ToString("yyyy/MM/d", CultureInfo.GetCultureInfo("en-US"));
            string urlFiveYearsAgo = baseUrl + woeid + "/" + dateToday.AddYears(-5).ToString("yyyy/MM/d", CultureInfo.GetCultureInfo("en-US"));

            List<string> weatherStatesNow = new List<string>();
            var requestContent = responseHelper.GetResponseContentAsync(urlNow, HttpMethod.Get);
            var itemsNow = responseHelper.GetArrayEnumerator(requestContent.Result);

            foreach (JsonElement i in itemsNow)
            {
                weatherStatesNow.Add(i.GetProperty(weatherStateNamePropertyName).GetString());
            }

            List<string> weatherStatesFiveYearsAgo = new List<string>();
            var stringContentForFiveYearsAgo = responseHelper.GetResponseContentAsync(urlFiveYearsAgo, HttpMethod.Get);
            var itemsForFiveYearsAgo = responseHelper.GetArrayEnumerator(stringContentForFiveYearsAgo.Result);

            foreach (JsonElement i in itemsForFiveYearsAgo)
            {
                weatherStatesFiveYearsAgo.Add(i.GetProperty(weatherStateNamePropertyName).GetString());
            }

            Assert.NotNull(weatherStatesNow.Intersect(weatherStatesFiveYearsAgo), "There is should be at least one match of {0} value of today and 5 years ago", weatherStateNamePropertyName);
        }

        private string GetSeason(string month)
        {
            switch (month)
            {
                case "12":
                case "1":
                case "2":
                    return "winter"; ;
                case "3":
                case "4":
                case "5":
                    return "spring";
                case "6":
                case "7":
                case "8":
                    return "summer";
                case "9":
                case "10":
                case "11":
                    return "autumn";

                default: 
                    return null;
            }
        }
    }
}