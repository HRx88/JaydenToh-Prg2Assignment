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
    class Cup :IceCream
    {
       public Cup() { }

        public Cup(string o, int s, List<Flavour> f, List<Topping> t):base(o, s, f, t) 
        {

        }

        public override double CalculatePrice()
        {
            double price = 0;
            foreach (Topping t in base.Toppings)
            {
                price += 1;

            }
            foreach (Flavour f in base.Flavours) // not sure if correct
            {
                if (f.Premium == true)
                {
                    price += (2 * f.Quantity);
                }
            }
            switch (base.Scoops)
            {
                case 2:
                    price += 5.50;
                    return price;
                case 3:
                    price += 6.50;
                    return price;
                default:
                    price += 4.00;
                    return price;
                    
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
