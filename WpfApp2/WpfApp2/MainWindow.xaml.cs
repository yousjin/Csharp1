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
        

        public MainWindow()
        {

            InitializeComponent();

            List<ContentsInfo> WAVVEdata = new List<ContentsInfo>();


            GetWAVVEPopdata(WAVVEdata, 2, 3);


            MessageBox.Show("전체 배열 크기 : " + WAVVEdata.Count);

            // 저장된 배열 확인 
            for(int l = 0; l < WAVVEdata.Count; l++)
            {
                MessageBox.Show((l+1).ToString() + " 번째 컨텐츠 이름 : " + WAVVEdata[l].Name);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 정보 : " + WAVVEdata[l].ConInfo);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 줄거리 : " + WAVVEdata[l].ConPlot);
                MessageBox.Show((l + 1).ToString() + " 번째 컨텐츠 이미지 인덱스 : " + WAVVEdata[l].ImgIndex);
            }



        }




    }


}
