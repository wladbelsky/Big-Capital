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
        private Double cost;
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
        public Currency(Currency currency)
        {
            this.name = currency.name;
            this.Cost = currency.Cost;
        }
        public Currency Clone() //clone, как конструктор копирования, вот только работает
        {
            return new Currency {name = this.name, cost = this.cost};
        }
        public override bool Equals(object obj)
        {
            if (! (obj is Currency)) return false;
            else
                return ((Currency) obj).name == name;
        }
        public override int GetHashCode()
        {
            return name.GetHashCode();//base.GetHashCode();
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
           return GetName() + "\t\t\t" + String.Format("{0:F8}", Cost);
        }

        public String GetName()
        {
            return name;
        }

    }

    class CurOwned : Currency   //Валюта пользователя
    {
        private Double count;

        public CurOwned(Currency cur, Double own)
        {
            name = cur.GetName();
            Cost = cur.Cost;
            count = own;
        }
        public CurOwned(String n, Double own, Double cost = 0) : base(n, cost)
        {
            count = own;
        }
        public override string GetCur()
        {
            return base.GetCur() + "\t" + Owned;
        }
        public Double Owned
        {
            get { return count; }
            set
            {
                if (value > 0)
                    count = value;
                else
                    count = 0;
            }
        }
    }
}
