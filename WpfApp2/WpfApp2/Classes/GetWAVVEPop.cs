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


namespace WpfApp2.Classes
{



    public class GetWAVVEPop
    {

        public class ContentsInfo
        {
            public string Name { get; set; }
            public string ConInfo { get; set; }
            public string ConPlot { get; set; }
            public string ImgIndex { get; set; }
        }


        public static void GetWAVVEPopdata(List<ContentsInfo> WAVVEdata, int pages, int Count_per_page)
        {
            string url = "https://www.wavve.com/list/VN500?api=apis.wavve.com%252Fes%252Fvod%252Fhotepisodes%253Forderby%253Dviewtime%2526contenttype%253Dvod%2526genre%253Dall%2526WeekDay%253Dall%2526uitype%253DVN500%2526uiparent%253DGN51-VN500%2526uirank%253D14%2526broadcastid%253D188792%2526offset%253D0%2526limit%253D20%2526uicode%253DVN500&came=home";
            string LoginUrl = "https://www.wavve.com/member/login?";

            // command 창, 웹 페이지 감춤 
            ChromeOptions options = new ChromeOptions();
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            options.AddArgument("--headless");

            IWebDriver driver = new ChromeDriver(driverService, options);

            // 실시간 인기 컨텐츠 페이지 이동 
            driver.Navigate().GoToUrl(url); 
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            IWebElement conElement;
            IWebElement imageElement;
            IWebElement buttonElement;

            string url_new = url;

            try
            {

                for (int j = 0; j < pages; j++)
                {
                    for (int i = 0; i < Count_per_page; i++)
                    {

                        int index = i + 1;
                        string conPath = "#contents > div.list-view-detail > div:nth-child(" + index.ToString() + ") > a";

                        conElement = driver.FindElement(By.CssSelector(conPath));

                        if (conElement.Displayed)
                        {

                            //이미지 다운로드
                            string imagePath = "#contents > div.list-view-detail > div:nth-child(" + index.ToString() + ") > a > div.thumb-image > img";
                            imageElement = driver.FindElement(By.CssSelector(imagePath));
                            string imageUrl = imageElement.GetAttribute("src");
                            string imageName = j.ToString() + "-" + i.ToString();
                            string fileName = "WavvePop_" + imageName + ".jpg";

                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileAsync(new Uri(imageUrl), fileName);

                            }

                            WAVVEdata.Add(new ContentsInfo() { ImgIndex = fileName });

                            //컨텐츠 상세 보기 페이지 접속 
                            conElement.Click();
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                            //HandleLoginPage(LoginUrl, url_new, driver);

                            //컨텐츠 세부 정보 추출 (ClassName 으로 추출)
                            string conGenre = "program-info-box";
                            conElement = driver.FindElement(By.ClassName(conGenre));

                            string runtimedigit = "";
                            string genrename = "";
                            char[] delimiterChars_genre = { '회', ')', '분' };
                            string[] Genretxt = conElement.Text.Split(delimiterChars_genre);
                            string digitcheck = Genretxt[2].Substring(Genretxt[2].Length - 3, 1);

                            if (digitcheck.All(char.IsDigit))
                            {
                                runtimedigit = Genretxt[2].Substring(Genretxt[2].Length - 3);
                                genrename = Genretxt[2].Substring(0, Genretxt[2].Length - 3);
                            }
                            else
                            {
                                runtimedigit = Genretxt[2].Substring(Genretxt[2].Length - 2);
                                genrename = Genretxt[2].Substring(0, Genretxt[2].Length - 2);
                            }
                            string genreresult = Genretxt[0] + "회 " + Genretxt[1] + ") " + genrename + " " + runtimedigit + "분 " + Genretxt[3];

                            WAVVEdata[i + Count_per_page * j].ConInfo = genreresult;
                           

                            //컨텐츠 줄거리 추출 (ClassName 으로 추출)
                            string moreButtonClassName = "info-toggle-button";
                            buttonElement = driver.FindElement(By.ClassName(moreButtonClassName));
                            buttonElement.Click();

                            string conPlot = "detail-view-content-synopsis";
                            conElement = driver.FindElement(By.ClassName(conPlot));


                            char[] delimiterChars = { '\n' };
                            string[] contentsPlot_all = conElement.Text.Split(delimiterChars);
                            string contentPlotResult = "";

                            // 더보기 쪽 내용 한 줄 이상 인 경우 
                            if (contentsPlot_all.Length > 1)
                            {
                                

                                string[] contentPlot = new string[contentsPlot_all.Length - 1];

                                for (int k = 1; k < contentsPlot_all.Length; k++)
                                {
                                    contentPlot[k - 1] = contentsPlot_all[k];
                                }

                                contentPlotResult = string.Join("\n", contentPlot);

                            }// 더보기 쪽 내용 한 줄인 경우
                            else
                            {

                                // 윗부분 줄거리 추출 
                                string conPlot_2 = "content-preview-box";
                                conElement = driver.FindElement(By.ClassName(conPlot_2));
                                contentPlotResult = conElement.Text;

                            }

                            // 컨텐츠 이름 추가 
                            WAVVEdata[i + Count_per_page * j].Name = contentsPlot_all[0];

                            WAVVEdata[i + j * Count_per_page].ConPlot = contentPlotResult;


                            //상세 페이지 빠져 나오기 
                            driver.Navigate().GoToUrl(url_new);
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                        }
                    }
                    //url 업데이트
                    int url_index = j + 2;
                    url_new = url + "&page=" + url_index.ToString();

                    driver.Navigate().GoToUrl(url_new); 
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                }
            }

            catch(NoSuchElementException)
            {
                driver.Quit();
                MessageBox.Show("i can't see anything");
            }

            driver.Quit();

        }

        //WAVVE 로그인 화면 뜰 경우 원래 url로 변경 
        private void HandleLoginPage(string LoginUrl, string url_new, IWebDriver driver)
        {

            string curUrl = driver.Url;
            curUrl = curUrl.Substring(0, LoginUrl.Length);

            if (curUrl.Equals(LoginUrl))
            {
                driver.Navigate().GoToUrl(url_new);
                Thread.Sleep(3000);
            }
        }

    }
  
}
