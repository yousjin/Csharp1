using System.Threading;
using System.Windows;
using System.Net;
//Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;
using System.IO;
using System;

namespace WpfApp2
{

    //실시간 인기 컨텐츠 정보 저장 
    public class ContentsInfo
    {
        public string Name { get; set; }
        public string ConInfo { get; set; }
        public string ConPlot { get; set; }
        public string ImgIndex { get; set; }
    }

    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {

            string url = "https://www.wavve.com/list/VN500?api=apis.wavve.com%252Fes%252Fvod%252Fhotepisodes%253Forderby%253Dviewtime%2526contenttype%253Dvod%2526genre%253Dall%2526WeekDay%253Dall%2526uitype%253DVN500%2526uiparent%253DGN51-VN500%2526uirank%253D14%2526broadcastid%253D188792%2526offset%253D0%2526limit%253D20%2526uicode%253DVN500&came=home";
            string LoginUrl = "https://www.wavve.com/member/login?";

            int Count_per_page = 4;
            InitializeComponent();

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url); // 실시간 인기 컨텐츠 페이지 이동 

            IWebElement conElement;
            IWebElement imageElement;
            IWebElement buttonElement;

            string url_new = url;

            List<ContentsInfo> ContentsList = new List<ContentsInfo>();


            try
            {
                

                for(int j =0;j<2; j++)
                {

   
                    for (int i = 0; i < Count_per_page; i++)
                    {

                        HandleLoginPage(LoginUrl, url_new, driver);
                        
                        int index = i + 1;
                        string conPath = "#contents > div.list-view-detail > div:nth-child(" + index.ToString() + ") > a";

                        conElement = driver.FindElement(By.CssSelector(conPath));


                        HandleLoginPage(LoginUrl, url_new, driver);


                        if (conElement.Displayed)
                        {

                            //이미지 다운로드
                            string imagePath = "#contents > div.list-view-detail > div:nth-child(" + index.ToString() + ") > a > div.thumb-image > img";
                            imageElement = driver.FindElement(By.CssSelector(imagePath));
                            string imageUrl = imageElement.GetAttribute("src");
                            string imageName = j.ToString() + "-" + i.ToString();
                            string fileName ="WavvePop_" + imageName + ".jpg";


                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileAsync(new Uri(imageUrl), fileName);

                            }

                            ContentsList.Add(new ContentsInfo() { ImgIndex = fileName });


                            //컨텐츠 상세 보기 페이지 접속 
                            conElement.Click();
                            Thread.Sleep(3000);

                            HandleLoginPage(LoginUrl, url_new, driver);


                            //컨텐츠 세부 정보 추출 (ClassName 으로 추출)
                            string conGenre = "program-info-box";
                            conElement = driver.FindElement(By.ClassName(conGenre));

                            ContentsList[i + Count_per_page * j].ConInfo = conElement.Text;
                            MessageBox.Show(ContentsList[i + Count_per_page * j].ConInfo);
                            Thread.Sleep(2000);


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
                                MessageBox.Show("줄거리 한줄 이상");


                                string[] contentPlot = new string[contentsPlot_all.Length - 1];

                                for(int k=1 ; k < contentsPlot_all.Length; k++)
                                {
                                    contentPlot[k - 1] = contentsPlot_all[k];
                                }

                                contentPlotResult = string.Join("\n", contentPlot);

                            }// 더보기 쪽 내용 한 줄인 경우
                            else 
                            {
                                
                                MessageBox.Show("줄거리 한줄");
                                

                                // 윗부분 줄거리 추출 
                                string conPlot_2 = "content-preview-box";
                                conElement = driver.FindElement(By.ClassName(conPlot_2));
                                contentPlotResult = conElement.Text;
                                
                            }

                            // 컨텐츠 이름 추가 
                            ContentsList[i + Count_per_page * j].Name = contentsPlot_all[0];
                            MessageBox.Show(ContentsList[i + Count_per_page * j].Name);

                            ContentsList[i + j * Count_per_page].ConPlot = contentPlotResult;
                            MessageBox.Show(ContentsList[i + Count_per_page * j].ConPlot);
                            Thread.Sleep(2000);

                            //상세 페이지 빠져 나오기 
                            driver.Navigate().GoToUrl(url_new);


                        }

                    }

                    //url 업데이트
                    int url_index = j + 2;
                    url_new = url + "&page=" + url_index.ToString();


                    driver.Navigate().GoToUrl(url_new);
                    //url 변경 후 1초 대기 
                    Thread.Sleep(1000);


                }

            }
            catch(NoSuchElementException)
            {
                driver.Quit();
                MessageBox.Show("i can't see anything");
            }


            MessageBox.Show("전체 배열 크기 : " + ContentsList.Count);

            // 저장된 배열 확인 
            for(int l = 0; l < ContentsList.Count; l++)
            {
                MessageBox.Show((l+1).ToString() + " 번째 컨텐츠 이름 : " + ContentsList[l].Name);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 : " + ContentsList[l].ConInfo);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 : " + ContentsList[l].ConPlot);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 인덱스 : " + ContentsList[l].ImgIndex);
            }

            driver.Quit();


            //Window에 첫번째 이미지 띄우기 
            


        }

        //로그인 페이지 뜨면 원래 페이지로 돌아가기 
        private void HandleLoginPage(string LoginUrl, string url_new, IWebDriver driver)
        {
            //WAVVE 로그인 화면 뜰 경우 원래 url로 변경 
            string curUrl = driver.Url;
            curUrl = curUrl.Substring(0, LoginUrl.Length);

            if (curUrl.Equals(LoginUrl))
            {
                MessageBox.Show("로그인 화면으로 전환");
                driver.Navigate().GoToUrl(url_new);
                Thread.Sleep(3000);
            }
        }


    }


}
