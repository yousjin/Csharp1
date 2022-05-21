using System.Threading;
using System.Windows;
using System.Net;
//Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace WpfApp2
{

    //실시간 인기 컨텐츠 정보 저장 
    public class ContentsInfo
    {
        public string Name { get; set; }
        public string ConInfo { get; set; }
        public string ConPlot { get; set; }
    }

    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {

            string url = "https://www.wavve.com/list/VN500?api=apis.wavve.com%252Fes%252Fvod%252Fhotepisodes%253Forderby%253Dviewtime%2526contenttype%253Dvod%2526genre%253Dall%2526WeekDay%253Dall%2526uitype%253DVN500%2526uiparent%253DGN51-VN500%2526uirank%253D14%2526broadcastid%253D188792%2526offset%253D0%2526limit%253D20%2526uicode%253DVN500&came=home";
          

            InitializeComponent();

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url); // 실시간 인기 컨텐츠 페이지 이동 

            IWebElement conElement;
            IWebElement cssElement;
            IWebElement buttonElement;

            string url_new = url;

            List<ContentsInfo> ContentsList = new List<ContentsInfo>();


            try
            {
                

                for(int j =0;j<5; j++)
                {

                    //한 페이지 내 컨텐츠 이름 배열(ContentsList)에 저장 
                    for (int i = 0; i < 3; i++)
                    {
                        int index = i + 1;
                        string conPath = "#contents > div.list-view-detail > div:nth-child(" + index.ToString() + ") > a";

                        conElement = driver.FindElement(By.CssSelector(conPath));
                        

                        if (conElement.Displayed)
                        {


                            //컨텐츠 상세 보기 창 접속 
                            conElement.Click();
                            Thread.Sleep(3000); 

                            //컨텐츠 세부 정보 추출 (ClassName 으로 추출)
                            string conGenre = "program-info-box";
                            cssElement = driver.FindElement(By.ClassName(conGenre));

                            ContentsList.Add(new ContentsInfo() { ConInfo = cssElement.Text });
                            ContentsList[i + 3 * j].ConInfo = cssElement.Text;
                            MessageBox.Show(ContentsList[i + 3 * j].ConInfo);
                            Thread.Sleep(2000);


                            //컨텐츠 줄거리 추출 (ClassName 으로 추출)
                            string moreButtonClassName = "info-toggle-button";
                            buttonElement = driver.FindElement(By.ClassName(moreButtonClassName));
                            buttonElement.Click();

                            string conPlot = "detail-view-content-synopsis";
                            cssElement = driver.FindElement(By.ClassName(conPlot));


                            char[] delimiterChars = { '\n' };
                            string[] contentsPlot_all = cssElement.Text.Split(delimiterChars);
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
                                cssElement = driver.FindElement(By.ClassName(conPlot_2));
                                contentPlotResult = cssElement.Text;
                                
                            }

                            // 컨텐츠 이름 추가 
                            ContentsList[i + 3 * j].Name = contentsPlot_all[0];
                            MessageBox.Show(ContentsList[i + 3 * j].Name);

                            ContentsList[i + j * 3].ConPlot = contentPlotResult;
                            MessageBox.Show(ContentsList[i + 3 * j].ConPlot);
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
                MessageBox.Show(l.ToString() + "번째 컨텐츠 이름 : " + ContentsList[l].Name);
                MessageBox.Show(l.ToString() + "번째 컨텐츠 정보 : " + ContentsList[l].ConInfo);
                MessageBox.Show(l.ToString() + "번째 컨텐츠 줄거리 : " + ContentsList[l].ConPlot);
            }



            driver.Quit();


        }
    }
}
