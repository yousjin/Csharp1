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
using static WpfApp2.Classes.GetNetflixPop;
using static WpfApp2.Classes.GetWatchaPop;
using static WpfApp2.Classes.GetTvingPop;
using static WpfApp2.Classes.GetDisneyPop;
using static WpfApp2.Classes.GetSearch;
using System.Threading.Tasks;

namespace WpfApp2
{

    //실시간 인기 컨텐츠 정보 저장 


    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {

            InitializeComponent();

            List<ContentsInfo> WAVVEdata = new List<ContentsInfo>();
            List<ContentsInfo> Netflixdata = new List<ContentsInfo>();
            List<ContentsInfo> Watchadata = new List<ContentsInfo>();
            List<ContentsInfo> Tivingdata = new List<ContentsInfo>();
            List<ContentsInfo> Disneydata = new List<ContentsInfo>();

            List<ReconInfo> Reconlist = new List<ReconInfo>();
            string conName = "나의 해방일지";
            GetSearchList(Reconlist, conName);

            for(int o=0; o<Reconlist.Count; o++)
            {
                MessageBox.Show((o + 1).ToString() + " 번째 컨텐츠 이름  : " + Reconlist[o].ConName);
                MessageBox.Show((o + 1).ToString() + " 번째 컨텐츠 링크  : " + Reconlist[o].ConHref);

                for(int u=0; u<Reconlist[o].OttList.Count; o++)
                {
                    MessageBox.Show(Reconlist[o].OttList[u]);
                }

            }



            //Parallel.Invoke(

            //    () => { GetWAVVEPopdata(WAVVEdata, 2, 3); },  // GetWAVVEPopdata(WAVVEdata, a, b) -> 컨텐츠 * (a*b) 개 
            //    () => { GetNetflixPopdata(Netflixdata, 5); }, // GetNetflixPopdata(Netflixdata, n) -> tv쇼 * n + 영화 * n 개
            //    () => { GetWatchaPopdata(Watchadata, 3); }, // GetWatchaPopdata(Watchadata , n) -> 컨텐츠 * (n*3) 개 
            //    () => { GetTivingPopdata(Tivingdata, 3); }, // GetTivingPopdata(Tivingdata, n) -> 드라마 * n + 예능 * n + 영화 * n 개 
            //    () => { GetDisneyPopdata(Disneydata, 5); });  // GetDisneyPopdata(Disneydata, n) -> TV 쇼 * n + 영화 * n 개


            //// 저장된 배열 확인 
            //for (int l = 0; l < Tivingdata.Count-3; l+=3)
            //{
            //    MessageBox.Show((l+1).ToString() + " 번째 컨텐츠 이름 WAVVE : " + WAVVEdata[l].Name);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 WAVVE : " + WAVVEdata[l].ConInfo);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 WAVVE : " + WAVVEdata[l].ConPlot);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 인덱스 WAVVE : " + WAVVEdata[l].ImgIndex);
            //}

            //for (int l = 0; l < Tivingdata.Count-3; l += 3)
            //{
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이름 Netflix : " + Netflixdata[l].Name);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 Netflix : " + Netflixdata[l].ConInfo);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 Netflix : " + Netflixdata[l].ConPlot);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 인덱스 Netflix : " + Netflixdata[l].ImgIndex);
            //}

            //for (int l = 0; l < Tivingdata.Count-3; l += 3)
            //{
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이름 Watcha : " + Watchadata[l].Name);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 Watcha : " + Watchadata[l].ConInfo);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 Watcha : " + Watchadata[l].ConPlot);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 인덱스 Watcha : " + Watchadata[l].ImgIndex);
            //}

            //for (int l = 0; l < Tivingdata.Count-3; l += 3)
            //{
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이름 Tiving : " + Tivingdata[l].Name);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 Tiving : " + Tivingdata[l].ConInfo);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 Tiving : " + Tivingdata[l].ConPlot);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 인덱스 Tiving : " + Tivingdata[l].ImgIndex);
            //}

            //for (int l = 0; l < Tivingdata.Count-3; l += 3)
            //{
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이름 Disney : " + Disneydata[l].Name);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 Disney : " + Disneydata[l].ConInfo);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 Disney : " + Disneydata[l].ConPlot);
            //    MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 인덱스 Disney : " + Disneydata[l].ImgIndex);
            //}


        }




    }


}
