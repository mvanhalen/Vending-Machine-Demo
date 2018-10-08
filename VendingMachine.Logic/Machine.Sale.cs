using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendingMachine.Logic
{
    /// <summary>
    /// Main class for vending machine sale functions
    /// </summary>
    public partial class Machine
    {
     

        /// <summary>
        /// Check if a sale possible.
        /// </summary>
        /// <param name="ProductId">Id value of the product object</param>
        /// <returns>True or False boolean</returns>
        public bool CanMakeSale(int ProductId)
        {
            if (CustomerCoinsValue() >= Products.Where(x => x.Id == ProductId && x.Stock > 0).Single().Price)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get coins for change. Using as less as possible
        /// </summary>
        /// <param name="ChangeNeeded">The change needed in cents</param>
        /// <returns>The coins to be returned to the customer</returns>
        public List<Coin> CalculateChange(int ChangeNeeded)
        {
            List<Coin> Change = new List<Coin>();

            //Loop through coins from highest to lowest value
            foreach (Coin coin in Coins.OrderByDescending(x => x.Cents))
            {
                //check if change needed is bigger then coin
                if ((decimal)(ChangeNeeded / coin.Cents) >= 1)
                {
                    //how many coins can be used
                    int Quantity = (int)Math.Round((decimal)(ChangeNeeded / coin.Cents),0);

                    //prepare coinchange
                    Coin CoinToAdd = new Coin { Cents = coin.Cents, Quantity = Quantity};

                    //add coin change
                    Change.Add(CoinToAdd);

                    //subtract used coins
                    ChangeNeeded -= CoinToAdd.TotalCents;
                }
            }

            return Change;

        }

        /// <summary>
        /// Make a product sale
        /// </summary>
        /// <param name="ProductId">The id of the product object</param>
        /// <returns>List of coins that is changed or the coins the customer has</returns>
        public List<Coin> MakeSale(int ProductId)
        {
            try
            {

                Product product = Products.Find(x => x.Id == ProductId);
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
                    if (TotalCents >= product.Price)
                    {
                        ChangeCoins = CalculateChange(TotalCents - product.Price);

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
                    return CustomerCoins;
                }
            }
            catch (Exception ex)
            {
                //Error return coins
                Console.WriteLine(ex.Message);
                return CustomerCoins;
            }
        }
    }
}
