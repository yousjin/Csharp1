using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp1.Interfaces;

namespace Csharp1.Classes
{
    partial class person 
    {

        //profull -> visual studio 기능

        private int height; // 필드(field), 멤버 변수 -> 소문자로 시작하는 것이 관례
        public int Height // 속성(properties)
        {
            get
            {
                return height;
            }

            set // private set으로 설정할 수도 있다.-> set이 은닉
            {
                if(value <= 0) // 대입한 값 저장 하는 키워드 : value
                {
                    Console.WriteLine("잘못된 값 입력");
                }

                height = value;
            }
        }
            

        //interface
        // public void print() = 0; -> 순수 가상 함수 in c++, 객체 생성 x, 상속 받는 유도 클래스에서 오버라이딩 해서 구현

        public string GetInformation()
        {
            return "키: " + Height + "몸무게: " + Weight;
        }
        

        
   









        public person(int height) //생성자
        {
            Height = height;
        }

        public person() //생성자
        {
            Height = 170;
        }

        //ctor -> 생성자 자동 생성



        //set, get 함수는 c#에서 사용하지 않아도 괜춘
        public void SetHeight(int height) // 메소드, 멤버 함수
        {
            if(height <= 0)
            {
                Console.WriteLine("잘못된 값 입력!");
                return;
            }

            this.height = height;
        }

        public int GetHeight()
        {
            return this.height;
        }

        //overriding
        public virtual int AddWeight(int addWeight)
        {
            Weight += addWeight;
            return Weight;
        }
    }
}
