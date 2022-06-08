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
    class GetTvingPop
    {
        public static void GetTivingPopdata(List<ContentsInfo> Tivingdata, int Count_per_page)
        {
            string url = "https://www.cjenm.com/ko/featured-contents/drama/";//드라마
            string url_new = "";
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

                for (int j = 0; j < 3; j++)
                {


                    for (int i = 0; i < Count_per_page; i++)
                    {
                        index = i + 1;

                        string imgPath = "#contents > section.thumbnail_list-section > div.section-inner > div > div.p-body > div > ul > li:nth-child(" + index.ToString() + ") > div > article > div > a > div.img-area > div > div.lazyload-wrapper > img";

                        conElement = driver.FindElement(By.CssSelector(imgPath));

                        if (conElement.Displayed)
                        {

                            //이미지 다운로드
                            string imgUrl = conElement.GetAttribute("src");
                            string imgName = j.ToString() + "-" + i.ToString();
                            string fileName = "TvingPop_" + imgName + ".jpg";

                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileAsync(new Uri(imgUrl), fileName);
                            }

                            Tivingdata.Add(new ContentsInfo() { ImgIndex = fileName });

                            //컨텐츠 주소 수집
                            string conPath = "#contents > section.thumbnail_list-section > div.section-inner > div > div.p-body > div > ul > li:nth-child(" + index.ToString() + ") > div > article > div > a";

                            conElement = driver.FindElement(By.CssSelector(conPath));
                            string conAddr = conElement.GetAttribute("href");

                            HrefList.Add(conAddr);

                        }

                    }

                    for (int k = j * Count_per_page; k < HrefList.Count; k++)
                    {

                        driver.Navigate().GoToUrl(HrefList[k]);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                        //컨텐츠 이름  
                        string conName = "#contents > section.detailbanner-section > div > div > div.p-header > div.title-box > h1";
                        conElement = driver.FindElement(By.CssSelector(conName));

                        Tivingdata[k].Name = conElement.Text;

                        //컨텐츠 세부정보
                        string conInfo = "category-list";
                        conElement = driver.FindElement(By.ClassName(conInfo));
                        Tivingdata[k].ConInfo = conElement.Text;

                        if (!Tivingdata[k].Name.Equals("샤크 : 더 비기닝"))
                        {
                            //컨텐츠 줄거리
                            string conmorebutton = "#contents > section.detailbanner-section > div > div > div.p-body > div > div.typo-area > ul > li:nth-child(1) > dl:nth-child(1) > dd.ellipsis_more > a";
                            conElement = driver.FindElement(By.CssSelector(conmorebutton));
                            conElement.Click();

                            string conSyn = "#intro1";
                            conElement = driver.FindElement(By.CssSelector(conSyn));
                            Tivingdata[k].ConPlot = conElement.Text;

                        }

                    }


                    if (j == 0)
                    {
                        url_new = "https://www.cjenm.com/ko/featured-contents/entertainment-shows/"; //예능

                    }
                    else if (j == 1)
                    {
                        url_new = "https://www.cjenm.com/ko/featured-contents/movies/"; //영화

                    }

                    driver.Navigate().GoToUrl(url_new);
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
