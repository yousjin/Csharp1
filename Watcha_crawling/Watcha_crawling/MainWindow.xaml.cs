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

            IWebDriver driver = new ChromeDriver();

            //왓챠 인기 컨텐츠 이동
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.FullScreen();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

            IWebElement conElement;
            IWebElement imageElement;

            List<ContentsInfo> ContentsData = new List<ContentsInfo>();

            try
            {

                for(int j=0; j<Lines; j++)
                {
                    LineIndex = j + 1;

                    for(int i=0; i<6; i++)
                    {
                        index = i + 1;
                        string conPath = "#root > div.css-ori2e1-NavManager > main > div.css-17lx1m-pageSideMargin-pageSideMargin > section > div:nth-child(" + LineIndex.ToString() + ") > div > ul > li:nth-child(" + index.ToString() + ") > article > a > div > div > img";

                        conElement = driver.FindElement(By.CssSelector(conPath));

                        IWebElement webelement = driver.FindElement(By.CssSelector(conPath));
                        Actions actions = new Actions(driver);
                        actions.MoveToElement(webelement);
                        actions.Perform();
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);

                        

                        if (conElement.Displayed)
                        {

                            //이미지 다운로드 
                            string imageUrl = conElement.GetAttribute("src");
                            int imageIndex = index + j * 6;
                            string imageName =imageIndex.ToString();
                            string fileName = "Watcha_" + imageName + ".jpg";

                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileAsync(new Uri(imageUrl), fileName);
                            }

                            ContentsData.Add(new ContentsInfo() { ImgIndex = fileName });

                            // 컨텐츠 상세 페이지로 이동
                            conElement.Click();
                            driver.Manage().Window.FullScreen();
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
                            

                            //컨텐츠 이름
                            string conName = "#root > div.css-cya6ac-NavManager > main > div.css-0 > header.css-wh4st9 > div > section > div.css-1qxls6i > h1";
                            
                            conElement = driver.FindElement(By.CssSelector(conName));
                            ContentsData[i + 6 * j].Name = conElement.Text;
                            MessageBox.Show(ContentsData[i + 6 * j].Name);
                            

                            //컨텐츠 세부정보
                            string conInfo = "css-7kpqz6";
                            conElement = driver.FindElement(By.ClassName(conInfo));
                            string age = conElement.Text.Substring(conElement.Text.Length - 2);
                            string age_not = conElement.Text.Substring(0, conElement.Text.Length - 4);
                            string conInforesult = age_not + " " + age;
                            ContentsData[i + 6 * j].ConInfo = conInforesult;
                            MessageBox.Show(ContentsData[i + 6 * j].ConInfo);

                            //컨텐츠 줄거리 
                            string conPlot = "//*[@id=\"root\"]/div[1]/main/div[1]/header[1]/div/section/div[2]/p[2]";
                            conElement = driver.FindElement(By.XPath(conPlot));

                            ContentsData[i + 6 * j].ConPlot = conElement.Text;
                            MessageBox.Show(ContentsData[i + 6 * j].ConPlot);

                            //전 페이지로 돌아가기
                            driver.Navigate().GoToUrl(url);
                            driver.Manage().Window.FullScreen();
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(50);
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
