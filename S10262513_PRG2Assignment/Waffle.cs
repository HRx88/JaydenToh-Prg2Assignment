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

        public Waffle(string o, int s, List<Flavour> f, List<Topping> t,string w= "Original") :base(o, s, f, t) 
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
                if (f.Premium==true)
                {
                    price += (2 * f.Quantity);
                }
            }
            switch (base.Scoops)
            {
                case 1:
                    if (WaffleFlavour != "Original")
                    {
                        price += (7.00 + 3);
                      
                    }
                    else
                    {
                        price += 7.00;
                        
                    }
                    break;
                case 2:
                    if (WaffleFlavour != "Original") 
                    {
                        price += 8.50+3;
                        
                    }
                    else
                    {
                        price += 8.50;
                        
                    }
                    break;
                case 3:
                    if (WaffleFlavour != "Original") 
                    {
                        price += 9.50 + 3;
                        
                    }
                    else
                    {
                        price += 9.50;
                        
                    }
                    break;
                
            }
            return price;
        }

        public override string ToString()
        {
            return base.ToString()+"\tWaffleFlavour: "+WaffleFlavour;
        }
    }
}
