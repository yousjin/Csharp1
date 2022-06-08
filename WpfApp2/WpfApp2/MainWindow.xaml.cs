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


        public class ReconInfo
        {
            public string ConName { get; set; }
            public string ConHref { get; set; }
            public bool IsNetflix { get; set; }
            public bool IsWAVVE { get; set; }
            public bool IsWatcha { get; set; }
            public bool IsTving { get; set; }
            public bool IsDisney { get; set; }

        }



        public MainWindow()
        {
            InitializeComponent();

            string urlHead = "https://www.justwatch.com/kr/검색?q=";
            string conName = "범죄도시";

            string url = urlHead + conName;

            List<ReconInfo> Reconlist = new List<ReconInfo>();
            List<ContentsInfo> ContentsList = new List<ContentsInfo>();

            IWebDriver driver = new ChromeDriver();

            //검색 결과 페이지로 이동
            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(70);

            IWebElement conElement;

            try
            {

                string recomCount = "#base > div.search-content > div > div > div.title-list-options > div > div.title-list-options__box-left > div";
                conElement = driver.FindElement(By.CssSelector(recomCount));

                //검색 연관 컨텐츠 수
                int conCount = int.Parse(conElement.Text.Substring(0, 1));
                
                for(int i=1; i<= conCount; i++)
                {
                    
                    string conNamePath = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(" + i.ToString() +") > ion-col:nth-child(2) > a > span.title-list-row__row-header-title";
                    conElement = driver.FindElement(By.CssSelector(conNamePath));

                    Reconlist.Add(new ReconInfo() { ConName = conElement.Text });

                    string conLink = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child("+ i.ToString() +") > ion-col:nth-child(2) > a";
                    conElement = driver.FindElement(By.CssSelector(conLink));

                    Reconlist[i - 1].ConHref = conElement.GetAttribute("href");

                    MessageBox.Show(Reconlist[i - 1].ConName + "  " + Reconlist[i - 1].ConHref);



                    //"#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(1) > ion-col:nth-child(2) > div:nth-child(2) > div.price-comparison--inline > div > div.price-comparison__grid__row.price-comparison__grid__row--rent > div.price-comparison__grid__row__holder > div > div > a > picture > img"
                    //"#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(1) > ion-col:nth-child(2) > div:nth-child(2) > div.price-comparison--inline > div > div.price-comparison__grid__row.price-comparison__grid__row--buy > div.price-comparison__grid__row__holder > div > div > a > picture > img"




                }

                if (Reconlist.Count == 0)
                {
                    MessageBox.Show("일치하는 콘텐츠를 찾을 수 없습니다.");
                }
                else //따로 함수로 만들기
                {
                    //일단 첫번째 검색 결과로 이동 (사용자 입력 받는 함수로 변경)
                    driver.Navigate().GoToUrl(Reconlist[0].ConHref);
                    driver.Manage().Window.FullScreen();
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                    //이미지 다운로드
                    string imgPath = "#base > div.jw-info-box > div > div.jw-info-box__container-sidebar > div > aside > div.hidden-xs.visible-sm.hidden-md.hidden-lg.title-sidebar__desktop > div > picture > img";
                    conElement = driver.FindElement(By.CssSelector(imgPath));

                    string imgurl = conElement.GetAttribute("src");
                    string fileName = "Search_new";

                    using(WebClient client = new WebClient())
                    {
                        client.DownloadFileAsync(new Uri(imgurl), fileName);
                    }

                    ContentsList.Add(new ContentsInfo() { ImgIndex = fileName });

                    //컨텐츠 이름
                    string searchName = "#base > div.jw-info-box > div > div.jw-info-box__container-content > div:nth-child(2) > div.title-block__container > div.title-block > div > h1";
                    conElement = driver.FindElement(By.CssSelector(searchName));

                    ContentsList[0].Name = conElement.Text;
                    MessageBox.Show(ContentsList[0].Name);

                    //컨텐츠 정보
                    string searchinfo = "#base > div.jw-info-box > div > div.jw-info-box__container-sidebar > div > aside > div.hidden-sm.visible-md.visible-lg.title-sidebar__desktop > div.title-info > div:nth-child(3) > div.detail-infos__value";
                    conElement = driver.FindElement(By.CssSelector(searchinfo));
                    string searchGenre = conElement.Text;

                    string runtimePath = "#base > div.jw-info-box > div > div.jw-info-box__container-sidebar > div > aside > div.hidden-sm.visible-md.visible-lg.title-sidebar__desktop > div.title-info > div:nth-child(4) > div.detail-infos__value";
                    conElement = driver.FindElement(By.CssSelector(runtimePath));
                    string runtime = conElement.Text;

                    string searchInfoResult = searchGenre + " " + runtime;
                    ContentsList[0].ConInfo = searchInfoResult;

                    MessageBox.Show(ContentsList[0].ConInfo);

                    //컨텐츠 줄거리
                    string searchPlot = "#base > div.jw-info-box > div > div.jw-info-box__container-content > div:nth-child(6) > div:nth-child(1) > div:nth-child(3) > p > span";
                    conElement = driver.FindElement(By.CssSelector(searchPlot));

                    ContentsList[0].ConPlot = conElement.Text;
                    MessageBox.Show(ContentsList[0].ConPlot);

                    //컨텐츠 ott
                    string ottPath = "#base > div.jw-info-box > div > div.jw-info-box__container-content > div:nth-child(2) > div:nth-child(2) > div > div.price-comparison--block";
                    conElement = driver.FindElement(By.CssSelector(ottPath));
                    string ottlist = conElement.Text;
                    char[] delimiterChars = { '\n' };
                    string[] ottArray = ottlist.Split(delimiterChars);

                    MessageBox.Show(ottlist);
                    MessageBox.Show(ottArray[0]);
                    MessageBox.Show(ottArray[1]);
                    MessageBox.Show(ottArray[2]);
                    MessageBox.Show(ottArray[3]);
                    MessageBox.Show(ottArray[4]);
                    MessageBox.Show(ottArray[5]);
                    MessageBox.Show(ottArray[6]);
                    MessageBox.Show(ottArray[7]);

                    //"#base > div.jw-info-box > div > div.jw-info-box__container-content > div:nth-child(2) > div:nth-child(2) > div > div.price-comparison--block > div > div.price-comparison__grid__row.price-comparison__grid__row--rent > div.price-comparison__grid__row__holder > div > div > a > picture > img"
                    //"#base > div.jw-info-box > div > div.jw-info-box__container-content > div:nth-child(2) > div:nth-child(2) > div > div.price-comparison--block > div > div.price-comparison__grid__row.price-comparison__grid__row--buy > div.price-comparison__grid__row__holder > div > div > a > picture > img"

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
