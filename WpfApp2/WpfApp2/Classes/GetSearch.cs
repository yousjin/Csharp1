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
    public class GetSearch
    {

        public class ReconInfo
        {
            public string ConName { get; set; }
            public string ConHref { get; set; }
            public List<string> OttList { get; set; }

        }

        //검색어(conName) -> 검색 결과 컨텐츠 목록 리스트 반환
        public static void GetSearchList(List<ReconInfo> Reconlist, string conName)
        {

            string urlHead = "https://www.justwatch.com/kr/검색?q=";
            string url = urlHead + conName;
            int index = -1;

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

                for (int i = 1; i <= conCount; i++)
                {


                    //컨텐츠 ott 확인
                    string otttest = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(" + i.ToString() + ") > ion-col:nth-child(2) > div:nth-child(2) > div.price-comparison--inline > div";
                    conElement = driver.FindElement(By.CssSelector(otttest));
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(80);
                    Thread.Sleep(1000);

                    char[] delimiterChars = { '\n' };
                    string[] ottArray = conElement.Text.Split(delimiterChars);
                    bool Isottcheck = true;
                    bool IsRent = false;
                    bool IsBuy = false;
                    bool IsStream = false;


                    //볼수 있는 ott 없는 경우
                    for (int k = 0; k < ottArray.Length; k++)
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
                            ottPath = "#base > div.search-content > div > div > div.title-list-row > ion-grid > div > ion-row:nth-child(" + i.ToString() + ") > ion-col:nth-child(2) > div:nth-child(2) > div.price-comparison--inline > div > div.price-comparison__grid__row.price-comparison__grid__row--rent > div.price-comparison__grid__row__holder > div > div > a > picture > img";
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
