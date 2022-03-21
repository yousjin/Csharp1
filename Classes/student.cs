using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp1.Interfaces;

namespace Csharp1.Classes
{
    class student : person, Iinformation //person 상속 == public 상속, 다중 상속 불가능, but 인터페이스는 제외
    {
        private int studentID;

        public int StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }

        public student(int h, int w, int id)
            : base(h, w) // 다중상속 불가능 -> 어차피 베이스 클래스는 하나이기 때문에 base로 일괄 처리 
        {
            StudentID = id;
        }

        public override int AddWeight(int addWeight)
        {
            Weight += addWeight-2;
            return Weight;
        }

        public string GetInformation()
        {
            return "키: " + Height + "몸무게: " + Weight + "학번: " + StudentID;
        }
    }
}
