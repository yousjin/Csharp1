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

    public class ContentsInfo
    {
        public string Name { get; set; }
    }

    public partial class MainWindow : Window
    {
        


        public MainWindow()
        {


            string url = "https://www.wavve.com/list/VN500?api=apis.wavve.com%252Fes%252Fvod%252Fhotepisodes%253Forderby%253Dviewtime%2526contenttype%253Dvod%2526genre%253Dall%2526WeekDay%253Dall%2526uitype%253DVN500%2526uiparent%253DGN51-VN500%2526uirank%253D14%2526broadcastid%253D188792%2526offset%253D0%2526limit%253D20%2526uicode%253DVN500&came=home";
            string url2 = "https://www.wavve.com/list/VN500?api=apis.wavve.com%252Fes%252Fvod%252Fhotepisodes%253Forderby%253Dviewtime%2526contenttype%253Dvod%2526genre%253Dall%2526WeekDay%253Dall%2526uitype%253DVN500%2526uiparent%253DGN51-VN500%2526uirank%253D14%2526broadcastid%253D188792%2526offset%253D0%2526limit%253D20%2526uicode%253DVN500&came=home&page=2";
            string url3 = "https://www.wavve.com/list/VN500?api=apis.wavve.com%252Fes%252Fvod%252Fhotepisodes%253Forderby%253Dviewtime%2526contenttype%253Dvod%2526genre%253Dall%2526WeekDay%253Dall%2526uitype%253DVN500%2526uiparent%253DGN51-VN500%2526uirank%253D14%2526broadcastid%253D188792%2526offset%253D0%2526limit%253D20%2526uicode%253DVN500&came=home&page=3";
            string className = "list-view-detail";
            string cssPathor = "#contents > div.list-view-detail > div:nth-child(1) > a > div.thumb-image > img";
            string cssPath2 = "#contents > div.list-view-detail > div:nth-child(2) > a > div.thumb-image > img";


            string cssPath_in_page_2 = "#contents > div.list-view-detail > div:nth-child(1) > a > div.thumb-image > img";
            string cssPath_in_page_3 = "#contents > div.list-view-detail > div:nth-child(1) > a > div.thumb-image > img";
            string idName = "contents";



            string page1_first = "https://www.wavve.com/player/vod?contentid=S01_E460828719.1&page=1";
            string page1_second = "https://www.wavve.com/player/vod?contentid=C2501_WPG2210240DA00013.1&page=1";



            InitializeComponent();

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);

            IWebElement cssElement;

            try
            {

                List<ContentsInfo> ContentsList = new List<ContentsInfo>();
                for(int j =0;j<10; j++)
                {


                    //한 페이지 내 컨텐츠 이름 배열(ContentsList)에 저장 
                    for (int i = 0; i < 3; i++)
                    {
                        int index = i + 1;
                        string cssPath = "#contents > div.list-view-detail > div:nth-child(" + index.ToString() + ") > a > div.thumb-image > img";

                        cssElement = driver.FindElement(By.CssSelector(cssPath));

                        if (cssElement.Displayed)
                        {


                            MessageBox.Show("i can see csspath");
                            Thread.Sleep(1000);

                            //string imgURL = cssElement.GetAttribute("src");

                            //인기 컨텐츠 이름 
                            //string imgName = cssElement.GetAttribute("alt");
                            string imgName = cssElement.GetAttribute("alt");

                            //컨텐츠 이미지 다운로드 
                            //WebClient downloader = new WebClient();
                            //downloader.DownloadFile(imgURL, "./Crawling_desk/Images");

                            ContentsList.Add(new ContentsInfo() { Name = imgName });
                            MessageBox.Show(ContentsList[i+3*j].Name);

                        }

                    }

                    //url 업데이트
                    int url_index = j + 2;
                    url = "https://www.wavve.com/list/VN500?api=apis.wavve.com%252Fes%252Fvod%252Fhotepisodes%253Forderby%253Dviewtime%2526contenttype%253Dvod%2526genre%253Dall%2526WeekDay%253Dall%2526uitype%253DVN500%2526uiparent%253DGN51-VN500%2526uirank%253D14%2526broadcastid%253D188792%2526offset%253D0%2526limit%253D20%2526uicode%253DVN500&came=home";
                    url = url + "&page=" + url_index.ToString();

                   

                    driver.Navigate().GoToUrl(url);

                    MessageBox.Show("This is current : " + url + "\r" + "This is url3 : " + url3);
                    MessageBox.Show(url.Equals(url3).ToString());

                    Thread.Sleep(1000);


                }

            }
            catch(NoSuchElementException)
            {
                MessageBox.Show("i can't see anything");
            }

            driver.Quit();


        }
    }
}
