using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10262513_PRG2Assignment
{
    abstract class IceCream
    {
        public string Option { get; set; }

        public int Scoops { get; set; }

        public List<Flavour> Flavours { get; set; } = new List<Flavour>();

        public List<Topping> Toppings { get; set; }= new List<Topping>();

        public IceCream()
        {
            
        }

        public IceCream(string o,int s,List<Flavour> f,List<Topping> t)
        {
            Option = o;
            Scoops = s;
            Flavours = f;
            Toppings = t;
        }

        public abstract double CalculatePrice();
        
            
        public override string ToString()
        {
            return "Option: "+Option+"\tScoops: "+Scoops;
        }
    }
}
