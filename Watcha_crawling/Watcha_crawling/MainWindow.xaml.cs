using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Net;
//Selenium Library
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace Watcha_crawling
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

            string url = "https://watcha.com/staffmades/409";
            int Lines = 3;
            int LineIndex = 0;
            int index = 0;


            ChromeOptions options = new ChromeOptions();
            ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            options.AddArgument("--headless");

            IWebDriver driver = new ChromeDriver(driverService, options);

            //왓챠 인기 컨텐츠 이동
            driver.Navigate().GoToUrl(url);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            IWebElement conElement;
            IWebElement imageElement;

            List<ContentsInfo> ContentsData = new List<ContentsInfo>();
            List<string> HrefList = new List<string>();

            try
            {

                //이미지 & href 얻기
                for (int j = 0; j < Lines; j++)
                {
                    LineIndex = j + 1;

                    for (int i = 0; i < 3; i++)
                    {
                        index = i + 1;
                        string conPath = "#root > div.css-ori2e1-NavManager > main > div.css-17lx1m-pageSideMargin-pageSideMargin > section > div:nth-child(" + LineIndex.ToString() + ") > div > ul > li:nth-child(" + index.ToString() + ") > article > a > div > div > img";

                        conElement = driver.FindElement(By.CssSelector(conPath));

                        if (conElement.Displayed)
                        {

                            //이미지 다운로드 
                            string imageUrl = conElement.GetAttribute("src");
                            int imageIndex = i + j * 3;
                            string imageName = imageIndex.ToString();
                            string fileName = "Watcha-" + imageName + ".jpg";

                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileAsync(new Uri(imageUrl), fileName);
                            }

                            ContentsData.Add(new ContentsInfo() { ImgIndex = fileName });

                            //href 추출 
                            string conhref = "#root > div.css-ori2e1-NavManager > main > div.css-17lx1m-pageSideMargin-pageSideMargin > section > div:nth-child(" + LineIndex.ToString() + ") > div > ul > li:nth-child(" + index.ToString() + ") > article > a";
                            conElement = driver.FindElement(By.CssSelector(conhref));

                            string href = conElement.GetAttribute("href");
                            HrefList.Add(href);

                        }

                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 200)");
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                    }
                }

                // href 로 컨텐츠 세부 정보 얻기 
                for(int k=0; k<HrefList.Count; k++)
                {
                    driver.Navigate().GoToUrl(HrefList[k]);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                    //컨텐츠 이름
                    string conName = "css-66jbd0";

                    conElement = driver.FindElement(By.ClassName(conName));
                    ContentsData[k].Name = conElement.Text;

                    //컨텐츠 세부정보
                    string conInfo = "css-1xhht53";
                    conElement = driver.FindElement(By.ClassName(conInfo));
                    ContentsData[k].ConInfo = conElement.Text;

                    //컨텐츠 줄거리 
                    string conPlot = "css-ieefh6";
                    conElement = driver.FindElement(By.ClassName(conPlot));
                    ContentsData[k].ConPlot = conElement.Text;

                }

            }
            catch (NoSuchElementException)
            {
                driver.Quit();
                MessageBox.Show("i can't see anything");
            }

            MessageBox.Show("저장된 배열 확인");
            for(int l=0; l<ContentsData.Count; l++)
            {
                MessageBox.Show(HrefList[l]);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이름 : " + ContentsData[l].Name);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 : " + ContentsData[l].ConInfo);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 : " + ContentsData[l].ConPlot);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 : " + ContentsData[l].ImgIndex);
            }


            driver.Quit();

        }
    }
}
