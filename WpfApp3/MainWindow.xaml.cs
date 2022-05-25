using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;




namespace WpfApp3
{

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
            InitializeComponent();

            string url_films = "https://top10.netflix.com/south-korea/films";
            string url_tv = "https://top10.netflix.com/south-korea/tv";

            string url = url_films;
            string curcon = "films-";

            int Count_per_page = 5;

            ChromeOptions options = new ChromeOptions();
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            options.AddArgument("--headless");

            IWebDriver driver = new ChromeDriver(driverService, options);

            IWebElement conElement;
            IWebElement imageElement;

            List<ContentsInfo> ContentsList = new List<ContentsInfo>();

            //한국 인기 films 로 이동 
            driver.Navigate().GoToUrl(url_films);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);



            try
            {


                for(int j=0; j<2; j++)
                {

                    for (int i = 0; i < Count_per_page; i++)
                    {
                        int index = i + 1;


                        string conPath = "//*[@id=\"weekly-lists\"]/div/div[5]/div/div[1]/div/table/tbody/tr[" + index.ToString() + "]";

                        conElement = driver.FindElement(By.XPath(conPath));

                        if (conElement.Displayed)
                        {

                            //이미지 다운로드
                            string imgPath = "//*[@id=\"weekly-lists\"]/div/div[3]/div[2]/div/ul/button[" + index.ToString() + "]/div[3]/div[2]/picture/img";
                            imageElement = driver.FindElement(By.XPath(imgPath));

                            string imgUrl = imageElement.GetAttribute("src");
                            string imageName = curcon + i.ToString();
                            string fileName = "NetflixPop_" + imageName + ".jpg";

                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileAsync(new Uri(imgUrl), fileName);
                            }

                            ContentsList.Add(new ContentsInfo() { ImgIndex = fileName });


                            string conId = conElement.GetAttribute("data-id");
                            string conAddr = "https://www.netflix.com/kr/title/" + conId;

                            //컨텐츠 상세 페이지로 이동 
                            driver.Navigate().GoToUrl(conAddr);
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                            //컨텐츠 이름 
                            string conName = "title-title";
                            conElement = driver.FindElement(By.ClassName(conName));

                            ContentsList[i + Count_per_page *j].Name = conElement.Text;
                            MessageBox.Show(ContentsList[i + Count_per_page *j].Name);

                            //컨텐츠 세부 정보 
                            string conDetail = "title-info-metadata-wrapper";
                            conElement = driver.FindElement(By.ClassName(conDetail));

                            ContentsList[i + Count_per_page*j].ConInfo = conElement.Text;
                            MessageBox.Show(ContentsList[i + Count_per_page*j].ConInfo);

                            //컨텐츠 줄거리 
                            string conSynop = "title-info-synopsis";
                            conElement = driver.FindElement(By.ClassName(conSynop));

                            ContentsList[i + Count_per_page *j].ConPlot = conElement.Text;
                            MessageBox.Show(ContentsList[i + Count_per_page *j].ConPlot);

                            //netfliex 10 page 로 돌아가기 
                            driver.Navigate().GoToUrl(url);
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                        }

                    }

                    url = url_tv;
                    curcon = "tv-";
                    driver.Navigate().GoToUrl(url);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                }

            }
            catch(NoSuchElementException)
            {
                driver.Quit();
                MessageBox.Show("i can't see anything");
            }

            MessageBox.Show("저장된 배열 확인");
            //저장된 배열 확인
            for(int l=0; l<ContentsList.Count; l++)
            {
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이름 : " + ContentsList[l].Name);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 : " + ContentsList[l].ConInfo);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 : " + ContentsList[l].ConPlot);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 인덱스 : " + ContentsList[l].ImgIndex);
            }




            driver.Quit();

            
           
  








        }
    }
}
