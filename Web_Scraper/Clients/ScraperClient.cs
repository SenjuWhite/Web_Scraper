using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;
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
            var phoneElements = driver.FindElements(By.CssSelector(".model-short-div.list-item--goods"));
            List<string> phoneInfoHTML = new List<string>();
            foreach (var phoneElement in phoneElements)
            {
                phoneInfoHTML.Add(phoneElement.GetAttribute("outerHTML"));
            }
            return phoneInfoHTML;

        }
        public Data GetPhoneInfo()
        {
            var data = new Data();
            
            var phoneInfoHTML = GetPhoneHTML();    
            List<Phone> phones = new List<Phone>();
            List<Store> stores = new List<Store>();
                foreach (var phoneElement in phoneInfoHTML)
                {
                Phone phoneInfo = new Phone();
                var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(phoneElement);
                    htmlDoc.OptionEmptyCollection = true;
                var id_Element = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'model-short-div') and contains(@class, 'list-item--goods')]")[0];
                phoneInfo.Id = id_Element.GetAttributeValue("id", string.Empty).Substring(3);
                phoneInfo.Model = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='u']").InnerText;
                phoneInfo.Year = htmlDoc.DocumentNode.SelectNodes("//span[@class='ib']").Count == 0 ? "Новинка" : htmlDoc.DocumentNode.SelectNodes("//span[@class='ib']")[0].InnerText;
                phoneInfo.Screen = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Екран:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Екран:')]")[0].InnerText;
                phoneInfo.Camera = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Камера:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Камера:')]")[0].InnerText;
                phoneInfo.Battery = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Акумулятор:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Акумулятор:')]")[0].InnerText;
                phoneInfo.Case = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Корпус:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Корпус:')]")[0].InnerText;
                var imgElement = htmlDoc.DocumentNode.SelectSingleNode("//img");
                phoneInfo.Image = imgElement.GetAttributeValue("src", "");
                var memoryString = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'ять:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'ять:')]")[0].InnerText;
                var processorString = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Процесор:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'Процесор:')]")[0].InnerText;
                var ramString = htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'ОЗП:')]").Count == 0 ? "unknown" : htmlDoc.DocumentNode.SelectNodes($"//*[contains(@title, 'ОЗП:')]")[0].InnerText;
                string ramPattern = @"ОЗП:(\d+(\.\d+)?)";
                string processorPattern = @"Процесор:(.+)";
                string screenPattern = @"Екран:(\d+\.?\d*)(?:\s|&nbsp;)*"".*?(\d+)(?:\s|&nbsp;)*Гц";
                string cameraPattern = @"(\d+(?:\.\d+)?)\s?&nbsp;МП";
                string memoryPattern = @"Пам'ять:(\d+)&nbsp;ГБ";
                string brandPattern = @"^\w+";
                var brandMatch = Regex.Match(phoneInfo.Model, brandPattern);
                var ramMatch = Regex.Match(ramString, ramPattern);
                var processorMatch = Regex.Match(processorString, processorPattern);
                var memoryMatch = Regex.Match(memoryString, memoryPattern);
                var screenMatch = Regex.Match(phoneInfo.Screen, screenPattern);
                phoneInfo.Brand = brandMatch.Success ? brandMatch.Value : "unknown";
                phoneInfo.RAM = ramMatch.Success ? ramMatch.Groups[1].Value : "unknown";
                phoneInfo.Processor = processorMatch.Success ? processorMatch.Groups[1].Value : "unknown"; 
                phoneInfo.Memory = memoryMatch.Success ? memoryMatch.Groups[1].Value : "unknown";
                if (screenMatch.Success)
                {
                     phoneInfo.Size = screenMatch.Groups[1].Value;
                   
                    if (!string.IsNullOrEmpty(screenMatch.Groups[2].Value)) {
                    
                    phoneInfo.Refresh_Rate = screenMatch.Groups[2].Value;}
                   
                }
                if (phoneInfo.Camera != "unknown")
                {
                    var cameraMatch = Regex.Matches(phoneInfo.Camera, cameraPattern);
                    if(cameraMatch.Count > 0)
                    {
                        phoneInfo.MainCameraMP = cameraMatch[0].Groups[1].Value;
                    }
                    if(cameraMatch.Count > 1)
                    {
                        phoneInfo.FrontCameraMP = cameraMatch[1].Groups[1].Value;
                    }
                }
                if(htmlDoc.DocumentNode.SelectNodes("//div[@class='l-pr-pd']").Count != 0)
                { 
                htmlDoc.LoadHtml(htmlDoc.DocumentNode.SelectNodes("//div[@class='l-pr-pd']")[0].InnerHtml);
                var storeElements = htmlDoc.DocumentNode.SelectNodes("//tr");
                    foreach (var storeElement in storeElements)
                    {
                        Store storeInfo = new Store();
                        storeInfo.PhoneId = phoneInfo.Id;
                        storeInfo.StoreName = storeElement.SelectNodes(".//u")[0].InnerText;
                        var links = storeElement.SelectNodes(".//a");
                        var regex = new Regex(@"&quot;(.*?)&quot;");
                        var match = regex.Match(links[0].GetAttributeValue("onmouseover", string.Empty));
                        storeInfo.PhoneLink = match.Groups[1].Value;
                        storeInfo.Price = links[1].InnerText.Replace("&nbsp;", "").Replace("грн.", "");
                        Console.WriteLine(storeInfo.PhoneLink);
                        Console.WriteLine(storeInfo.Price);

                        Console.WriteLine("\n\n\n\n\n\n\n\n\n");
                        stores.Add(storeInfo);
                    }
                }
                Console.WriteLine("\n\n\n\n\n\n\n");
                Console.WriteLine(phoneInfo.Brand);
                //Console.WriteLine(phoneInfo.Id);
                //Console.WriteLine(phoneInfo.RAM);
                //Console.WriteLine(phoneInfo.Processor);
                //Console.WriteLine(phoneInfo.Memory);
                //Console.WriteLine(phoneInfo.MainCameraMP);
                //Console.WriteLine(phoneInfo.FrontCameraMP);
                //Console.WriteLine(phoneInfo.Size);
                //Console.WriteLine(phoneInfo.Refresh_Rate + "\n\n\n\n");
                phones.Add(phoneInfo);
                }
            data.Phone = phones;
            data.Store = stores;
 
            return data;
        }
     

    }
}
