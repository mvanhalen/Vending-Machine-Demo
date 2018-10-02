using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Logic
{
    public partial class Machine
    {
        public List<Product> Products { get; set; }
        public List<Coin> Coins { get; set; }
        public List<Coin> CustomerCoins { get; set; }

        /// <summary>
        /// Base Machine class. Products and coins are added via overides
        /// </summary>
        public Machine()
        {
            //set products
            Products = new List<Product>();
                      
            //set coins
            Coins = new List<Coin>();

            //set customer coins
            CustomerCoins = new List<Coin>();

        }

        /// <summary>
        /// Start the machine with base products and coins
        /// </summary>
        /// <param name="products">List of products</param>
        /// <param name="coins">List os coins</param>
        public Machine(List<Product> products, List<Coin> coins)
        {
            Products = products;
            Coins = coins;
            CustomerCoins = new List<Coin>();
        } 

        public int CoinInsert(Coin coin)
        {
            if (CustomerCoins.Count == 0)
            {
                CustomerCoins.Add(coin);
            }
            else
            {
                if(CustomerCoins.Where(x=> x.Cents == coin.Cents).Count() == 0)
                {
                    CustomerCoins.Add(coin);
                }
                else
                {
                    CustomerCoins.Where(x => x.Cents == coin.Cents).Single().Quantity++;
                    CustomerCoins.Where(x => x.Cents == coin.Cents).Single().TotalCents += coin.Cents;
                }
            }

            return CustomerCoinsValue();
        }

        public int CustomerCoinsValue()
        {
            if(CustomerCoins.Count == 0)
            {
                return 0;
            }
            else
            {
                return CustomerCoins.Sum(x => x.TotalCents);
            }
        }

        public bool CanMakeSale(int ProductId)
        {
            if(CustomerCoinsValue() >= Products.Where(x=> x.Id== ProductId && x.Stock > 0).Single().Price)
            {
                return true;
            }
            return false;
        }

        public List<Coin> CalculateChange(int changeneeded)
        {
            List<Coin> change = new List<Coin>();

            //Loop through coins from highest to lowest value
            foreach(Coin coin in Coins.OrderByDescending(x=> x.Cents))
            {
                //check if change needed is bigger then coin
                if ((changeneeded / coin.Cents > 1))
                {
                    //how many coins can be used
                    int Qty = (int)Math.Round((decimal)changeneeded / coin.Cents, 0);

                    //prepare coinchange
                    Coin cointoadd = new Coin { Cents = coin.Cents, Quantity = Qty, TotalCents = Qty * coin.Cents };
                    
                    //add coin change
                    change.Add(cointoadd);
                    
                    //subtract used coins
                    changeneeded -= cointoadd.TotalCents;
                }
            }
            
            return change;

        }

        public List<Coin> MakeSale(int ProductId)
        {
            Product prod = Products.Find(x => x.Id == ProductId);
            int TotalCents = CustomerCoins.Sum(x => x.TotalCents);
            if (CanMakeSale(ProductId))
            {
                //reduce product
                Products.Find(x => x.Id == ProductId).Stock--;

                //add to user coins to machine
                CustomerCoins.ForEach(coin =>
                {
                    CoinInsert(coin);

                });


                //check if change is needed
                if( TotalCents >= prod.Price)
                {
                    //return change
                    return CalculateChange(TotalCents - prod.Price);
                }

            }

            //cancel
            return Coins;
        }

    }

    
}
