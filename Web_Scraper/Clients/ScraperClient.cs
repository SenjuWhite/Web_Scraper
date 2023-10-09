using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Xml.XPath;
using Web_Scraper.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Web_Scraper.Clients
{
    public class Scraper
    {
       
        private IWebDriver driver;

        public Scraper()
        {
            driver = new ChromeDriver();
        }
        private void NavigateToUrl()
        {
            driver.Navigate().GoToUrl("https://ek.ua/ua/list/122/");
            Thread.Sleep(3000);

            while (true)
            {
                try
                {
                    var loadMoreButton = driver.FindElement(By.ClassName("list-more-div"));
                    loadMoreButton.Click();
                    Thread.Sleep(2000);
                }
                catch (NoSuchElementException)
                {
                    break;
                }
            }
        }
        private List<string> GetPhoneHTML()
        {
            NavigateToUrl();
            var phoneElements = driver.FindElements(By.ClassName("model-short-block"));
            List<string> phoneInfoHTML = new List<string>();
            foreach (var phoneElement in phoneElements)
            {
                phoneInfoHTML.Add(phoneElement.GetAttribute("outerHTML"));
            }
            return phoneInfoHTML;
            //foreach (var phoneElement in phoneInfo)
            //{
            //    Console.WriteLine(phoneElement+"\n\n\n\n\n\n\n");
            //}


        }
        public List<PhoneInfo> GetPhoneInfo()
        {
            var phoneInfoHTML = GetPhoneHTML();    
            List<PhoneInfo> phones = new List<PhoneInfo>();
                foreach (var phoneElement in phoneInfoHTML)
                {
                PhoneInfo phoneInfo = new PhoneInfo();
                var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(phoneElement);
                    htmlDoc.OptionEmptyCollection = true;
                phoneInfo.Model = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='u']").InnerText;
                phoneInfo.Year = htmlDoc.DocumentNode.SelectNodes("//span[@class='ib']").Count == 0 ? "Новинка" : htmlDoc.DocumentNode.SelectNodes("//span[@class='ib']")[0].InnerText;
                phoneInfo.Screen = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Екран:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Екран:')]")[0].InnerText;
                    phoneInfo.Camera = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Камера:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Камера:')]")[0].InnerText;
                    phoneInfo.Memory = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'ять:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'ять:')]")[0].InnerText;
                    phoneInfo.Processor = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Процесор:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Процесор:')]")[0].InnerText;
                    phoneInfo.RAM = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'ОЗП:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'ОЗП:')]")[0].InnerText;
                    phoneInfo.Battery = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Акумулятор:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Акумулятор:')]")[0].InnerText;
                    phoneInfo.Case = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Корпус:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Корпус:')]")[0].InnerText;
                    var imgElement = htmlDoc.DocumentNode.SelectSingleNode("//img");
                phoneInfo.Image = imgElement.GetAttributeValue("src", "");
                    phones.Add(phoneInfo);
                }
                return phones;
          
            //foreach (var phoneElement in phones)
            //{
            //    Console.WriteLine(phoneElement.Model);
            //    Console.WriteLine(phoneElement.Year);
            //    Console.WriteLine(phoneElement.Screen);
            //    Console.WriteLine(phoneElement.Camera);
            //    Console.WriteLine(phoneElement.Memory);
            //    Console.WriteLine(phoneElement.Processor);
            //    Console.WriteLine(phoneElement.RAM);
            //    Console.WriteLine(phoneElement.Battery);
            //    Console.WriteLine(phoneElement.Case);
            //    Console.WriteLine(phoneElement.Image);
            //    Console.WriteLine("\n\n\n\n\n\n\n\n");


            //}

        }
     

    }
}
