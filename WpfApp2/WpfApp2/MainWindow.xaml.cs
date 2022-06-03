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


            string url = "https://www.cjenm.com/ko/featured-contents/drama/";//드라마
            string url_new = "";

            int Count_per_page = 5;
            int index = 0;


            IWebDriver driver = new ChromeDriver();

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

                        MessageBox.Show("j count: " + j.ToString());
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
                            MessageBox.Show(Contentdata[i + j*Count_per_page].ImgIndex);

                            //컨텐츠 주소 수집
                            string conPath = "#contents > section.thumbnail_list-section > div.section-inner > div > div.p-body > div > ul > li:nth-child(" + index.ToString() + ") > div > article > div > a";

                            conElement = driver.FindElement(By.CssSelector(conPath));
                            string conAddr = conElement.GetAttribute("href");

                            HrefList.Add(conAddr);
                            MessageBox.Show(HrefList[i + j*Count_per_page]);

                        }

                    }

                    for (int k = j*Count_per_page; k < HrefList.Count; k++)
                    {

                        driver.Navigate().GoToUrl(HrefList[k]);
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                        //컨텐츠 이름  
                        string conName = "#contents > section.detailbanner-section > div > div > div.p-header > div.title-box > h1";
                        conElement = driver.FindElement(By.CssSelector(conName));

                        Contentdata[k].Name = conElement.Text;
                        MessageBox.Show(Contentdata[k].Name);

                        //컨텐츠 세부정보
                        string conInfo1 = "#contents > section.detailbanner-section > div > div > div.p-header > div.title-box > div > ul > li:nth-child(1)";
                        string conInfo2 = "#contents > section.detailbanner-section > div > div > div.p-header > div.title-box > div > ul > li:nth-child(2)";
                        string conInfo3 = "#contents > section.detailbanner-section > div > div > div.p-header > div.title-box > div > ul > li:nth-child(3)";
                        

                        conElement = driver.FindElement(By.CssSelector(conInfo1));
                        string conGenre1 = conElement.Text;
                        conElement = driver.FindElement(By.CssSelector(conInfo2));
                        string conGenre2 = conElement.Text;
                        conElement = driver.FindElement(By.CssSelector(conInfo3));
                        string conlength = conElement.Text;

                        string conInfoResult = conGenre1 + " " + conGenre2 + " " + conlength;

                        
                        Contentdata[k].ConInfo = conInfoResult;
                        MessageBox.Show(Contentdata[k].ConInfo);

                        //테스트 용
                        string all_test = "#contents > section > div > div > div.p-header > div.title-box > div > ul";
                        conElement = driver.FindElement(By.CssSelector(all_test));
                        

                        //컨텐츠 줄거리
                        string conmorebutton = "#contents > section.detailbanner-section > div > div > div.p-body > div > div.typo-area > ul > li:nth-child(1) > dl:nth-child(1) > dd.ellipsis_more > a";
                        conElement = driver.FindElement(By.CssSelector(conmorebutton));
                        conElement.Click();

                        string conSyn = "#intro1";
                        conElement = driver.FindElement(By.CssSelector(conSyn));
                        Contentdata[k].ConPlot = conElement.Text;
                        MessageBox.Show(Contentdata[k].ConPlot);
                    }


                    if (j == 1)
                    {
                        url_new = "https://www.cjenm.com/ko/featured-contents/entertainment-shows/"; //예능

                    }
                    else
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



