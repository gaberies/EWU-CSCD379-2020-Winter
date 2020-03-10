using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SecretSanta.Web.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlogEngine.Web.Tests
{
    [TestClass]
    public class AuthorTests
    {
        [NotNull]
        public TestContext? TestContext { get; set; }

        [NotNull]
        private IWebDriver? Driver { get; set; }

        private static Process? ApiHostProcess { get; set; }

        private static Process? WebHostProcess { get; set; }

        public static string AppUrl { get; } = "http://localhost:5002";

        private static int _MockUserId;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            ApiHostProcess = Process.Start("dotnet.exe", "run -p ..\\..\\..\\..\\..\\src\\SecretSanta.Api\\SecretSanta.Api.csproj");
            WebHostProcess = Process.Start("dotnet.exe", "run -p ..\\..\\..\\..\\..\\src\\SecretSanta.Web\\SecretSanta.Web.csproj");
            ApiHostProcess.WaitForExit(8000);

            GenerateUser();
        }

        public static async Task GenerateUser()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(AppUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            UserClient userClient = new UserClient(httpClient);
            var users = await userClient.GetAllAsync();
            if (users.Count == 0)
            {
                UserInput userInput = new UserInput
                {
                    FirstName = "Test",
                    LastName = "User"
                };

                User user = await userClient.PostAsync(userInput);
                _MockUserId = user.Id;
            }
            httpClient.Dispose();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (!(ApiHostProcess is null))
            {
                ApiHostProcess.Kill();
                ApiHostProcess.CloseMainWindow();
                ApiHostProcess.Close();
            }

            if (!(WebHostProcess is null))
            {
                WebHostProcess.Kill();
                WebHostProcess.CloseMainWindow();
                WebHostProcess.Close();
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {

            string browser = "Chrome";
            switch (browser)
            {
                case "Chrome":
                    Driver = new ChromeDriver();
                    break;
                default:
                    Driver = new ChromeDriver();
                    break;
            }
            Driver.Manage().Timeouts().ImplicitWait = new System.TimeSpan(0, 0, 10);
        }

        [TestMethod]
        public void VerifySiteIsUp()
        {
            Driver.Navigate().GoToUrl(new Uri(AppUrl));
            string text = Driver.FindElement(By.XPath("/html/body/section/div/p")).Text;
            Assert.IsTrue(text.Contains("Welcome to your secret santa app"));
        }

        [TestMethod]
        public void DoAssignment()
        {
            Driver.Navigate().GoToUrl(new Uri(AppUrl));
            Driver.FindElement(By.Id("gifts-link")).Click();
            Driver.FindElement(By.Id("create-new-gift-button")).Click();
            Driver.FindElement(By.Id("gift-title-input")).SendKeys("Mock Title");
            Driver.FindElement(By.Id("gift-description-input")).SendKeys("Mock Description");
            Driver.FindElement(By.Id("gift-url-input")).SendKeys("Mock Url");
            Driver.FindElement(By.Id("gift-select")).Click();
            Driver.FindElement(By.Id($"{_MockUserId}")).Click();
            Driver.FindElement(By.Id("gift-submit-button")).Click();

            Screenshot screenShot = ((ITakesScreenshot)Driver).GetScreenshot();

            screenShot.SaveAsFile("./testScreenShot.png");

/*            IWebElement sut = Driver.FindElement(By.Link);

            Assert.IsTrue(text.Contains("Welcome to your secret santa app"));*/
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            Driver.Quit();
        }
    }

    public static class StringRegExExtension
    {
        static public string RegexReplace(this string input, string findPattern, string replacePattern)
        {
            return Regex.Replace(input, findPattern, replacePattern);
        }
    }
}