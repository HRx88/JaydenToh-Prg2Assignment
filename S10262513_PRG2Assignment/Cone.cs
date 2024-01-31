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
    class Cone : IceCream
    {
        public bool Dipped { get; set; }

        public Cone()
        {
            
        }

        public Cone(string o, int s, List<Flavour> f, List<Topping> t,bool d=false):base(o, s, f, t)
        {
            Dipped = d;
        }

        public override double CalculatePrice()
        {
            double price=0;
            foreach (Topping t in base.Toppings)
            {
                price += 1;

            }
            foreach (Flavour f in base.Flavours)
            {
                if (f.Premium == true)
                {
                    price += (2 * f.Quantity);
                }
            }
            switch (base.Scoops)
            {
                case 1:
                    if (Dipped == true)
                    {
                        price += 4.00 + 2;
                        
                    }
                    else
                    {
                        price += 4.00;
                        
                    }
                    break;
                case 2:
                    if (Dipped==true)//need put ==true???
                    {
                        price += 5.50 + 2;
                        
                    }
                    else
                    {
                        price += 5.50;
                        
                    }
                    break;
                case 3:
                    if (Dipped == true)
                    {
                        price += 6.50 + 2;
                        
                    }
                    else
                    {
                        price += 6.50;
                      
                    } 
                    break;
            }
            return price;
        }

        public override string ToString()
        {
            return base.ToString()+"\tDipped: "+Dipped;
        }
    }
}
