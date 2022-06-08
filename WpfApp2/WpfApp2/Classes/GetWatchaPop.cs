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
    class GetWatchaPop
    {

        public static void GetWatchaPopdata(List<ContentsInfo> Watchadata, int Lines)
        {

            string url = "https://watcha.com/staffmades/409";
            int LineIndex = 0;
            int index = 0;


            ChromeOptions options = new ChromeOptions();
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            options.AddArgument("--headless");

            IWebDriver driver = new ChromeDriver(driverService, options);

            //왓챠 인기 컨텐츠 이동
            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            IWebElement conElement;

            List<string> HrefList = new List<string>();

            try
            {

                //이미지 & href 얻기
                for (int j = 0; j < Lines; j++)
                {
                    LineIndex = j + 1;

                    for (int i = 0; i < 3; i++)
                    {
                        index = i + 1;
                        string conPath = "#root > div.css-ori2e1-NavManager > main > div.css-17lx1m-pageSideMargin-pageSideMargin > section > div:nth-child(" + LineIndex.ToString() + ") > div > ul > li:nth-child(" + index.ToString() + ") > article > a > div > div > img";

                        conElement = driver.FindElement(By.CssSelector(conPath));

                        if (conElement.Displayed)
                        {

                            //이미지 다운로드 
                            string imageUrl = conElement.GetAttribute("src");
                            int imageIndex = i + j * 3;
                            string imageName = imageIndex.ToString();
                            string fileName = "Watcha-" + imageName + ".jpg";

                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileAsync(new Uri(imageUrl), fileName);
                            }

                            Watchadata.Add(new ContentsInfo() { ImgIndex = fileName });

                            //href 추출 
                            string conhref = "#root > div.css-ori2e1-NavManager > main > div.css-17lx1m-pageSideMargin-pageSideMargin > section > div:nth-child(" + LineIndex.ToString() + ") > div > ul > li:nth-child(" + index.ToString() + ") > article > a";
                            conElement = driver.FindElement(By.CssSelector(conhref));

                            string href = conElement.GetAttribute("href");
                            HrefList.Add(href);

                        }

                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 200)");
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                    }
                }

                // href 로 컨텐츠 세부 정보 얻기 
                for (int k = 0; k < HrefList.Count; k++)
                {
                    driver.Navigate().GoToUrl(HrefList[k]);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                    //컨텐츠 이름
                    string conName = "css-66jbd0";

                    conElement = driver.FindElement(By.ClassName(conName));
                    Watchadata[k].Name = conElement.Text;

                    //컨텐츠 세부정보
                    string conInfo = "css-1xhht53";
                    conElement = driver.FindElement(By.ClassName(conInfo));
                    Watchadata[k].ConInfo = conElement.Text;

                    //컨텐츠 줄거리 
                    string conPlot = "css-ieefh6";
                    conElement = driver.FindElement(By.ClassName(conPlot));
                    Watchadata[k].ConPlot = conElement.Text;

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



