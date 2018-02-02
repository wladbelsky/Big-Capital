using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Big_Capital.Capital_Logic
{
    public class Mining
    {
        List<vCart> carts = new List<vCart>();

        public Mining()
        {

        }
        public void Block(PlayerInterface sender)
        {
            Int32 totalPower;
            Random rnd = new Random();
            if(rnd.Next(0, 100) < totalPower)
            {
               // sender.AddCurOwned(new CurOwned(sender.));
            }
        }
    }
    abstract class Eqt
    {
        protected Double price;
        protected String name;
        public Eqt(String name, Double price)
        {
            this.price = price;
            this.name = name;
        }
        public Eqt(Eqt eqt)
        {
            this.name = eqt.name;
            this.price = eqt.price;
        }
        class vCard : Eqt
        {
            
            Int32 power;
            public vCard(String name, Double price, Int32 power) : base(name, price)
            {
                this.power = power;
            }
            public vCard(vCard card) : base(card)
            {
                this.power = card.power;
            }
        }
        class mBoard : Eqt
        {
            Int32 countCards;
            public mBoard(String name, Double price, List<vCart> countCarts) : base(name, price)
            {
                this.countCards = countCarts;
            }
        }
    }
}
