using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    class Currency
    {
        protected String name;
        Double cost;
        public Currency()
        {
            name = "";
            cost = 0;
        }
        public Currency(String n, Double cost = 0)
        {
            name = n;
            this.cost = cost;
        }
        public Currency Clone() //clone, как конструктор копирования, вот только работает
        {
            return new Currency {name = this.name, cost = this.cost};
        }

        public Double Cost
        {
            get { return cost; }
            set
            {
                if (value > 0)
                    cost = value;
                else
                    cost = 0;
            }
        }

        public virtual string GetCur()
        {
           return GetName() + "\t\t\t" + Cost;
        }

        public String GetName()
        {
            return name;
        }

    }
}
