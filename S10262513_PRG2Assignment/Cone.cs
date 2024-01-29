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
            foreach(Flavour f in base.Flavours)  // not sure if correct
            {
                price += f.Premium ? 2 * f.Quantity : f.Quantity;
            }
            switch (base.Scoops)
            {
                case 2:
                    if (Dipped)//need put ==true???
                    {
                        price += 5.50 + 2;
                        return price;
                    }
                    else
                    {
                        price += 5.50;
                        return price;
                    }
                case 3:
                    if (Dipped)
                    {
                        price += 6.50 + 2;
                        return price;
                    }
                    else
                    {
                        price += 6.50;
                        return price;
                    }
                default:
                    if (Dipped)
                    {
                        price += 4.00 + 2;
                        return price;
                    }
                    else
                    {
                        price += 4.00;
                        return price;
                    }
            }
        }

        public override string ToString()
        {
            return base.ToString()+"\tDipped: "+Dipped;
        }
    }
}
