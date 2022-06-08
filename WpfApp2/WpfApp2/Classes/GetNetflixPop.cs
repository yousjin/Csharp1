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
    class GetNetflixPop
    {


        public static void GetNetflixPopdata(List<ContentsInfo> Netflixdata, int Count_per_page)
        {
            string url_films = "https://top10.netflix.com/south-korea/films";
            string url_tv = "https://top10.netflix.com/south-korea/tv";
            string url = url_films;
            string curcon = "films-";


            ChromeOptions options = new ChromeOptions();
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            options.AddArgument("--headless");

            IWebDriver driver = new ChromeDriver(driverService, options);

            IWebElement conElement;
            IWebElement imageElement;


            //한국 인기 films 로 이동 
            driver.Navigate().GoToUrl(url_films);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            try
            {
                for (int j = 0; j < 2; j++)
                {

                    for (int i = 0; i < Count_per_page; i++)
                    {

                        int index = i + 1;
                        string conPath = "//*[@id=\"weekly-lists\"]/div/div[5]/div/div[1]/div/table/tbody/tr[" + index.ToString() + "]";

                        conElement = driver.FindElement(By.XPath(conPath));
                        if (conElement.Displayed)
                        {
                            //이미지 다운로드
                            string imgPath = "//*[@id=\"weekly-lists\"]/div/div[3]/div[2]/div/ul/button[" + index.ToString() + "]/div[3]/div[2]/picture/img";
                            imageElement = driver.FindElement(By.XPath(imgPath));

                            string imgUrl = imageElement.GetAttribute("src");
                            string imageName = curcon + i.ToString();
                            string fileName = "NetflixPop_" + imageName + ".jpg";

                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileAsync(new Uri(imgUrl), fileName);
                            }

                            Netflixdata.Add(new ContentsInfo() { ImgIndex = fileName });


                            string conId = conElement.GetAttribute("data-id");
                            string conAddr = "https://www.netflix.com/kr/title/" + conId;

                            //컨텐츠 상세 페이지로 이동 
                            driver.Navigate().GoToUrl(conAddr);
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                            //컨텐츠 이름 
                            string conName = "title-title";
                            conElement = driver.FindElement(By.ClassName(conName));

                            Netflixdata[i + Count_per_page * j].Name = conElement.Text;
                           

                            //컨텐츠 세부 정보 
                            string conDetail = "title-info-metadata-wrapper";
                            conElement = driver.FindElement(By.ClassName(conDetail));

                            Netflixdata[i + Count_per_page * j].ConInfo = conElement.Text;
                            

                            //컨텐츠 줄거리 
                            string conSynop = "title-info-synopsis";
                            conElement = driver.FindElement(By.ClassName(conSynop));

                            Netflixdata[i + Count_per_page * j].ConPlot = conElement.Text;
                           

                            //netfliex 10 page 로 돌아가기 
                            driver.Navigate().GoToUrl(url);
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                        }
                    }
                    url = url_tv;
                    curcon = "tv-";
                    driver.Navigate().GoToUrl(url);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

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
