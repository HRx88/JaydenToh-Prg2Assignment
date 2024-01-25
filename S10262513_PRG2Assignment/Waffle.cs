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
    class Waffle :IceCream
    {
        public string WaffleFlavour { get; set; }

        public Waffle()
        {
            
        }

        public Waffle(string o, int s, List<Flavour> f, List<Topping> t,string w= "original") :base(o, s, f, t) 
        {
            WaffleFlavour = w;
        }

        public override double CalculatePrice()
        {
            double price = 0;
            foreach (Topping t in base.Toppings)
            {
                price += 1;

            }
            foreach (Flavour f in base.Flavours)
            {
                price += f.Premium ? 2 * f.Quantity : f.Quantity;
            }
            switch (base.Scoops)
            {
                case 2:
                    if (WaffleFlavour != "original") // Check if a waffle flavor is selected
                    {
                        price += 8.50+3;
                        return price;
                    }
                    else
                    {
                        price += 8.50;
                        return price;
                    }
                case 3:
                    if (WaffleFlavour != "original") // Check if a waffle flavor is selected
                    {
                        price += 9.50 + 3;
                        return price;
                    }
                    else
                    {
                        price += 9.50;
                        return price;
                    }
                default:
                    if (WaffleFlavour != "original") // Check if a waffle flavor is selected
                    {
                        price += 7.00 + 3;
                        return price;
                    }
                    else
                    {
                        price += 7.00;
                        return price;
                    }

            }
        }

        public override string ToString()
        {
            return base.ToString()+"\tWaffleFlavour: "+WaffleFlavour;
        }
    }
}
