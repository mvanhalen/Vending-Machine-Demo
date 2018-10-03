using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Machine
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coin"></param>
        public void CoinInsertMachine(Coin coin)
        {
            if (Coins.Count == 0)
            {
                Coins.Add(coin);
            }
            else
            {
                if (Coins.Where(x => x.Cents == coin.Cents).Count() == 0)
                {
                    Coins.Add(coin);
                }
                else
                {
                    Coins.Where(x => x.Cents == coin.Cents).Single().Quantity++;
                    Coins.Where(x => x.Cents == coin.Cents).Single().TotalCents += coin.Cents;
                }
            }

        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coin"></param>
        /// <returns></returns>
        public int CoinInsertCustomer(Coin coin)
        {
            if (CustomerCoins.Count == 0)
            {
                CustomerCoins.Add(coin);
            }
            else
            {
                if (CustomerCoins.Where(x => x.Cents == coin.Cents).Count() == 0)
                {
                    CustomerCoins.Add(coin);
                }
                else
                {
                    CustomerCoins.Where(x => x.Cents == coin.Cents).Single().Quantity++;
                    CustomerCoins.Where(x => x.Cents == coin.Cents).Single().TotalCents += coin.Cents;
                }
            }

            //reset
            ChangeCoins = new List<Coin>();

            return CustomerCoinsValue();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="coin"></param>
       public void RemoveCoin(Coin coin)
        {
            if (Coins.Where(x => x.Cents == coin.Cents).Count() > 0)
            {
                Coins.Where(x => x.Cents == coin.Cents).Single().Quantity--;
                Coins.Where(x => x.Cents == coin.Cents).Single().TotalCents -= coin.Cents;
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CustomerCoinsValue()
        {
            if (CustomerCoins.Count == 0)
            {
                return 0;
            }
            else
            {
                return CustomerCoins.Sum(x => x.TotalCents);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public bool CanMakeSale(int ProductId)
        {
            if (CustomerCoinsValue() >= Products.Where(x => x.Id == ProductId && x.Stock > 0).Single().Price)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changeneeded"></param>
        /// <returns></returns>
        public List<Coin> CalculateChange(int changeneeded)
        {
            List<Coin> change = new List<Coin>();

            //Loop through coins from highest to lowest value
            foreach (Coin coin in Coins.OrderByDescending(x => x.Cents))
            {
                //check if change needed is bigger then coin
                if ((decimal)(changeneeded / coin.Cents) >= 1)
                {
                    //how many coins can be used
                    int Qty = (int)Math.Round((decimal)(changeneeded / coin.Cents),0);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public List<Coin> MakeSale(int ProductId)
        {
            try
            {

                Product prod = Products.Find(x => x.Id == ProductId);
                int TotalCents = CustomerCoins.Sum(x => x.TotalCents);
                bool SaleSuccess = false;
                if (CanMakeSale(ProductId))
                {
                    //reset change
                    ChangeCoins = new List<Coin>();

                    //reduce product
                    Products.Find(x => x.Id == ProductId).Stock--;

                    //add to user coins to machine
                    CustomerCoins.ForEach(coin =>
                    {
                        CoinInsertMachine(coin);

                    });

                    //check if change is needed
                    if (TotalCents >= prod.Price)
                    {
                        ChangeCoins = CalculateChange(TotalCents - prod.Price);

                        //remove coins from current stash
                        ChangeCoins.ForEach(coin =>
                        {
                            RemoveCoin(coin);
                        });

                        //return change

                    }

                    //reset usercoins sale is make
                    CustomerCoins = new List<Coin>();
                    SaleSuccess = true;

                }

                if (SaleSuccess)
                {
                    // return change
                    return ChangeCoins;
                }
                else
                {
                    //cancel
                    return Coins;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Error return coins
                return Coins;
            }
        }
    }
}
