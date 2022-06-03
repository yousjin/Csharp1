using System.Threading;
using System.Windows;
using System.Net;
using System.Linq;
//Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.IO;
using System;
using static WpfApp2.Classes.GetWAVVEPop;

namespace WpfApp2
{

    //실시간 인기 컨텐츠 정보 저장 


    public partial class MainWindow : Window
    {


        public class ContentsInfo
        {
            public string Name { get; set; }
            public string ConInfo { get; set; }
            public string ConPlot { get; set; }
            public string ImgIndex { get; set; }
        }



        public MainWindow()
        {
            InitializeComponent();

            string url_drama = "https://www.cjenm.com/ko/featured-contents/drama/";
            string url_enter = "https://www.cjenm.com/ko/featured-contents/entertainment-shows/";
            string url_films = "https://www.cjenm.com/ko/featured-contents/movies/";

            string url = url_drama;

            int Count_per_page = 5;
            int index = 0;


            IWebDriver driver = new ChromeDriver();

            //티빙 드라마 -> 예능 -> 영화
            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            IWebElement conElement;

            List<string> HrefList = new List<string>();
            List<ContentsInfo> Contentdata = new List<ContentsInfo>();

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

                            Contentdata.Add(new ContentsInfo() { ImgIndex = fileName });
                            MessageBox.Show(Contentdata[i].ImgIndex);

                            //컨텐츠 주소 수집
                            string conPath = "#contents > section.thumbnail_list-section > div.section-inner > div > div.p-body > div > ul > li:nth-child(" + index.ToString() + ") > div > article > div > a";

                            conElement = driver.FindElement(By.CssSelector(conPath));
                            string conAddr = conElement.GetAttribute("href");

                            HrefList.Add(conAddr);
                            MessageBox.Show(HrefList[i]);

                        }

                    }

                    for (int k = 0; k < HrefList.Count; k++)
                    {

                        driver.Navigate().GoToUrl(HrefList[k]);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                        //컨텐츠 이름  
                        string conName = "#contents > section.detailbanner-section > div > div > div.p-header > div.title-box > h1";
                        conElement = driver.FindElement(By.CssSelector(conName));

                        MessageBox.Show(conElement.Text);
                        Contentdata[k].Name = conElement.Text;

                        //컨텐츠 세부정보
                        string conInfo = "#contents > section.detailbanner-section > div > div > div.p-header > div.title-box > div > ul";
                        conElement = driver.FindElement(By.CssSelector(conInfo));

                        string conGenre = conElement.Text.Substring(0, 3) + " ";
                        string contest = conElement.Text.Substring(conElement.Text.Length - 3);
                        MessageBox.Show(conGenre);

                        //컨텐츠 줄거리
                        string conmorebutton = "#contents > section.detailbanner-section > div > div > div.p-body > div > div.typo-area > ul > li:nth-child(1) > dl:nth-child(1) > dd.ellipsis_more > a";
                        conElement = driver.FindElement(By.CssSelector(conmorebutton));
                        conElement.Click();

                        string conSyn = "#intro1";
                        conElement = driver.FindElement(By.CssSelector(conSyn));
                        MessageBox.Show(conElement.Text);
                    }

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
