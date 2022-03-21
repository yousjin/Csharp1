using System; //기능 사용 되면 밝게 바뀜
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp1.Classes;
using System.Collections;
using Csharp1.Interfaces;

// using namespace std;  #include 와 비슷

namespace Csharp1

{

    delegate int MyDelegate(int a, int b); // Delegate 는 타입이다.



    class Calculator
    {
        public int Plus(int a, int b)
        {
            return a + b;
        }
        public int Minus(int a, int b)
        {
            return a - b;
        }
    }

    delegate void MyDelegate_2(string message);

    class EventManager
    {
        public event MyDelegate_2 eventCall;
        public void DoSomething(int num)
        {
            int temp = num % 10;
            if(temp %2 == 0)
            {
                eventCall(num.ToString());
            }
        }
    }




    class Program
    {

        static int numberThree = 3; // program 의 멤버 변수

        static void MyHandler(string msg)
        {
            Console.WriteLine(msg);
        }


        // static -> program 객체 만들지 않아도 main 사용 가능
        // Program p; <-사전 작업     p.Main();   필요 x
        static void Main()
        {

            Calculator calc = new Calculator();
            int c = calc.Plus(1, 2);

            Console.WriteLine(c);

            MyDelegate Callback;
            Callback = new MyDelegate(calc.Plus);

            Console.WriteLine(Callback(3,4)); //calc.Plus 처럼 사용 가능하다.(대리 하고 있다.)

            Callback = new MyDelegate(calc.Minus);
            Console.WriteLine(Callback(3,4));


            Callback += calc.Plus; // 등록

            //Evnet----------------------------------------------------

            EventManager em = new EventManager();
            em.DoSomething(10);
            em.DoSomething(7);

            em.eventCall += new MyDelegate_2(MyHandler);

            for(int i=0; i<30; i++)
            {
                em.DoSomething(i);
            }

            






            // std::cout << "hello world"    in c++
            Console.WriteLine("Hello World");

            int numberOne = 20;
            var numberTwo = 23.0f; //auto -> var : 변수의 타입을 미정(우변과 같은 타입을 갖는다)

            Console.WriteLine(numberOne.GetType());   //변수는 클래스의 객체이다. int 라는 것 또한 클래스

            Console.WriteLine(numberThree); // static 함수에서는 static 변수에만 접근 가능


            //---------- 값 형식 ---------//
            int aVal = 35;
            int bVal;

            bVal = aVal;

            bVal = 25;

            Console.WriteLine(aVal);
            Console.WriteLine(bVal);

            //-----------참조 형식 ----------//
            MyInt aRef = new MyInt();
            aRef.val = 35;
            MyInt bRef;

            bRef = aRef;

            bRef.val = 25;

            Console.WriteLine(aRef.val);
            Console.WriteLine(bRef.val);

            //메모리 주소 같음 (얕은 복사)
            //주소값은 stack에 저장되고, 실제 값은 heap에 저장 된다. -> bRef.val 값을 바꾸면 aRef.val 값도 바뀐다.


            {
                int three = 10;

            } // 닫는 순간 메모리 공간 해제

            // Console.WriteLine(three); three라는 공간은 이미 해체 되었기 때문에 오류

            // C# 은 C++ 과 달리 가비지 컬렉터 존재

            // C#에서 변수의 타입은 클래스이다. 그래서 변수는 객체 이다., object 가 모든 데이터 형식의 조상
            // 기본 클래스가 사용되는 곳에는 유도 클래스도 사용 가능하다.




            //Class--------------------------------------------------------------------------------------------------

            //Csharp1.Classes.person p;
            person p = new person(180);
            //Console.WriteLine(p.Height); -> private 으로 변경 -> 직접 접근 x

            Console.WriteLine(SimpleMath.Add(1.23, 3.45)); //Add 는 static 함수 이므로 객체 생성할 필요 없이 바로 쓸수 있음.

            //Array------------------------------------------------------------------------------------------------------------------
            int[] numbers = new int[3]; //참조 형식
            numbers[0] = 1;
            numbers[1] = 3;
            numbers[2] = 10;

            //(마치 동적 할당처럼, 변수 크기를 사용할 수 있다.)
            int arraySize = 5;
            int[] numbers2 = new int[arraySize];

            printArray(numbers);// arr.Length 를 통해 size를 직접 입력할 필요 없음.
            printArray2(numbers);

            Array.Sort(numbers);// 정렬 내장 함수


            ArrayList arrList = new ArrayList();
            arrList.Add(3);
            arrList.Add(2);
            arrList.Add(1);

            Console.WriteLine(arrList[0]);
            arrList.RemoveAt(1);
            arrList.Insert(0, 5);

            //printArray(arrList) -> arrayList 이기 때문에 불가능, arrayList.count 로 길이 체크 가능

            //___properties-------------------------------------------------------------------------------------
            Console.WriteLine(p.GetHeight());

            p.SetHeight(-1);
            Console.WriteLine(p.GetHeight());

            //property 추가 후
            Console.WriteLine(p.Height); // property 로써 Height, //get 이 호출

            p.Height = -1; // set이 호출, -1이 value에 전달
            Console.WriteLine(p.Height);


            student s = new student(180, 80, 201720083);
            Console.WriteLine(s.StudentID);

            //오버로딩-----------------------------------------
            Console.WriteLine(SimpleMath.Add(1,2));
            Console.WriteLine(SimpleMath.Add(1.12, 2.34));

            //++---------------------------------------------------
            List<double> numbersList = new List<double>();

            numbersList.Add(1.0);
            numbersList.Add(2.0);

            //타입 변환 method
            double[] numbersListToArray = numbersList.ToArray();

            //오버라이딩------------------------------------------------
            //상속 관계가 있을때 메소드를 다르게 동작시키는 것
            person p_1 = new person(180, 80);
            p_1.AddWeight(5);

            Console.WriteLine(p_1.Weight);

            student s_1 = new student(180, 80, 201720083);
            s_1.AddWeight(5);

            person p2;
            p2 = s_1;
            p2.AddWeight(5); // student의 AddWeight 사용 

            //인터페이스--------------------------------------------------
            PrintInformation(p_1); // Iinformation 을 사용하는 곳에는 person 도 사용 할 수 있다. person 또한 유도 클래스 이기 때문에

            PrintInformation(s_1);

            Car c = new Car();
            c.weight = 2000;

            PrintInformation(c); // Iinforamtion을 상속받았기 때문에

            

        }

        static void printArray(int[] arr)
        {
            for(int i=0; i<arr.Length; i++)
            {
                Console.WriteLine(arr[i]);
            }
        }

        //IEnumerable -> interface : foreach 사용 가능 (하나씩 꺼내서 사용)
        static void printArray2(IEnumerable arr)
        {
            foreach(object element in arr)
            {
                Console.WriteLine(element.ToString());
            }
        }



        public class MyInt
        {
            public int val;
        }

        static void PrintInformation(Iinformation info)
        {
            Console.WriteLine(info.Getinformation());
        }

        static class SimpleMath()


    }



    class SimpleMath
    {
        public static int Add(int n1, int n2)
        {
            return n1 + n2;
        }

        //오버로딩
        public static double Add(double n1, double n2)
        {
            return n1 + n2;
        }
    }
}
