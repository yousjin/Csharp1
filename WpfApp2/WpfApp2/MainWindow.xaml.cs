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
            public List<string> OttList { get; set; }

        }



        public MainWindow()
        {
            InitializeComponent();

            string urlHead = "https://www.justwatch.com/kr/검색?q=";
            string conName = "나의 해방일지";

            string url = urlHead + conName;
            int index = -1;

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


                    //컨텐츠 ott 확인
                    string otttest = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(" + i.ToString() + ") > ion-col:nth-child(2) > div:nth-child(2) > div.price-comparison--inline > div";
                    conElement = driver.FindElement(By.CssSelector(otttest));
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(80);
                    Thread.Sleep(1000);

                    MessageBox.Show(conElement.Text);

                    char[] delimiterChars = { '\n' };
                    string[] ottArray = conElement.Text.Split(delimiterChars);
                    bool Isottcheck = true;
                    bool IsRent = false;
                    bool IsBuy = false;
                    bool IsStream = false;


                    //볼수 있는 ott 없는 경우
                    for (int k=0; k<ottArray.Length; k++)
                    {

                        ottArray[k] = ottArray[k].Substring(0, ottArray[k].Length - 1);


                        if (ottArray[k].Equals("알림신청"))
                        {
                            Isottcheck = false;
                        }
                        else if (ottArray[k].Equals("대여"))
                        {
                            IsRent = true;
                        }
                        else if (ottArray[k].Equals("구매"))
                        {
                            IsBuy = true;
                        }
                        else if (ottArray[k].Equals("스트리밍"))
                        {
                            IsStream = true;
                        }
                        else
                        { }

                    }

                    //볼 수 있는 ott 존재하는 경우
                    if (Isottcheck)
                    {
                        index += 1;

                        //컨텐츠 이름 저장
                        string conNamePath = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(" + i.ToString() + ") > ion-col:nth-child(2) > a > span.title-list-row__row-header-title";
                        conElement = driver.FindElement(By.CssSelector(conNamePath));

                        Reconlist.Add(new ReconInfo() { ConName = conElement.Text });

                        //컨텐츠 상세 정보 링크 수집
                        string conLink = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(" + i.ToString() + ") > ion-col:nth-child(2) > a";
                        conElement = driver.FindElement(By.CssSelector(conLink));
                        Reconlist[index].ConHref = conElement.GetAttribute("href");

                        List<string> ottchecklist = new List<string>();
                        string ottPath = "";
                        Reconlist[index].OttList = new List<string>();
                        
                        //컨텐츠 ott 확인 
                        if (IsRent)
                        {
                            ottPath = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child("+ i.ToString() + ") > ion-col:nth-child(2) > div:nth-child(2) > div.price-comparison--inline > div > div.price-comparison__grid__row.price-comparison__grid__row--rent > div.price-comparison__grid__row__holder > div > div > a > picture > img";
                            conElement = driver.FindElement(By.CssSelector(ottPath));

                            Reconlist[index].OttList.Add(conElement.GetAttribute("alt"));

                        }
                        if (IsBuy)
                        {
                            ottPath = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(" + i.ToString() + ") > ion-col:nth-child(2) > div:nth-child(2) > div.price-comparison--inline > div > div.price-comparison__grid__row.price-comparison__grid__row--buy > div.price-comparison__grid__row__holder > div > div > a > picture > img";
                            conElement = driver.FindElement(By.CssSelector(ottPath));

                            Reconlist[index].OttList.Add(conElement.GetAttribute("alt"));
                        }
                        if (IsStream)
                        {
                            ottPath = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(" + i.ToString() + ") > ion-col:nth-child(2) > div:nth-child(2) > div.price-comparison--inline > div > div.price-comparison__grid__row.price-comparison__grid__row--stream > div.price-comparison__grid__row__holder > div > div > a > picture > img";
                            conElement = driver.FindElement(By.CssSelector(ottPath));

                            Reconlist[index].OttList.Add(conElement.GetAttribute("alt"));
                        }

                        for(int l=0; l< Reconlist[index].OttList.Count; l++)
                        {
                            MessageBox.Show(Reconlist[index].OttList[l]);
                        }


                    }

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
