using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp1.Interfaces;

namespace Csharp1.Classes
{
    class Car : Iinformation
    {
        public int weight;

        public string Getinformation()
        {
            return "자동차 무게: " + weight;
        }
    }
}
