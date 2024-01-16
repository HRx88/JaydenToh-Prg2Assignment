using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262513_PRG2Assignment
{
    //==========================================================
    // Student Number : S10262513
    // Student Name : Tan Hong Rong
    // Partner Name : Jayden Toh Xuan Ming
    //==========================================================
    class Topping
    {
        public string Type { get; set; }

        public Topping()
        {
            
        }
        public Topping(string t)
        {
            Type = t;
        }

        public override string ToString()
        {
            return "Type: "+Type;
        }
    }
}
