using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Net;
//Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using static WpfApp2.Classes.GetWAVVEPop;

namespace WpfApp2.Classes
{
    class GetDisneyPop
    {
        public static void GetDisneyPopdata(List<ContentsInfo> Disneydata, int Count_per_page)
        {

            string url = "https://flixpatrol.com/top10/disney/";
            int index = 0;


            ChromeOptions options = new ChromeOptions();
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            options.AddArgument("--headless");

            IWebDriver driver = new ChromeDriver(driverService, options);

            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            IWebElement conElement;

            List<string> HrefList = new List<string>();

            try
            {

                for (int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < Count_per_page; i++)
                    {

                        index = i + 1;

                        string conPath = "//*[@id=\"disney-" + (j + 1).ToString() + "\"]/div[2]/div/div/table/tbody/tr[" + index.ToString() + "]/td[2]/a";

                        conElement = driver.FindElement(By.XPath(conPath));

                        if (conElement.Displayed)
                        {
                            string conAddr = conElement.GetAttribute("href");
                            HrefList.Add(conAddr);

                        }

                    }

                }

                for (int k = 0; k < HrefList.Count; k++)
                {

                    driver.Navigate().GoToUrl(HrefList[k]);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                    // 이미지 추출하기
                    string imgPath = "body > div.content.mt-4 > div > div.w-40.md\\:w-1\\/5.mb-4.mx-auto.md\\:mt-1.flex-shrink-0 > div > picture > img";
                    conElement = driver.FindElement(By.CssSelector(imgPath));
                    string imgsource = conElement.GetAttribute("src");
                    string fileName = "Disney_" + k.ToString() + ".jpg";

                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFileAsync(new Uri(imgsource), fileName);
                    }

                    Disneydata.Add(new ContentsInfo() { ImgIndex = fileName });

                    //컨텐츠 이름
                    string conName = "body > div.content.mt-4 > div > div.flex-grow > div.mb-6 > div.md\\:flex.items-baseline.justify-between > h1";
                    conElement = driver.FindElement(By.CssSelector(conName));

                    Disneydata[k].Name = conElement.Text;

                    //컨텐츠 정보
                    string conInfo = "body > div.content.mt-4 > div > div.flex-grow > div.mb-6 > div.flex.flex-wrap.text-gray-500 > div";
                    conElement = driver.FindElement(By.CssSelector(conInfo));

                    char[] delimiterChars = { '|', '\n' };
                    string[] conInfoArray = conElement.Text.Split(delimiterChars);
                    string conInforesult = conInfoArray[0].Substring(0, conInfoArray[0].Length - 1);
                    for (int l = 3; l < conInfoArray.Length - 3; l += 3)
                    {
                        conInforesult = conInforesult + " " + conInfoArray[l].Substring(0, conInfoArray[l].Length - 1);

                    }
                    conInforesult = conInforesult + " " + conInfoArray[conInfoArray.Length - 1];


                    Disneydata[k].ConInfo = conInforesult;

                    //컨텐츠 줄거리 
                    string conPlot = "body > div.content.mt-4 > div > div.flex-grow > div.lg\\:flex.lg\\:space-x-8.justify-between > div:nth-child(1) > div.card.-mx-content > div:nth-child(1)";
                    conElement = driver.FindElement(By.CssSelector(conPlot));

                    string conPlotresult = conElement.Text;
                    Disneydata[k].ConPlot = conPlotresult;

                }

            }
            catch (NoSuchElementException)
            {
                driver.Quit();
                MessageBox.Show("i can't see anything");
            }

            driver.Quit();



        }

    }
}
