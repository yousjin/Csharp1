using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp1.Classes
{
    partial class person //partial 여러 파일에서 person 클래스 동시에 구현
    {
        private int weight;
        public int Weight
        {
            set { weight = value;  }
            get { return weight; }
        }

        public person(int h, int w)
        {
            Height = h;
            Weight = w;
        }
    }
}
