using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Logic
{

   public partial class Machine
   {
        /// <summary>
        /// Start machine with base values
        /// </summary>
        /// <returns></returns>
        public static Machine StartMachine()
        {
            //set products
            List<Product> products = new List<Product>();
            products.Add(new Product { Id = 1, Title = "Tea", Price = 130, Stock = 10 });
            products.Add(new Product { Id = 2, Title = "Expresso", Price = 180, Stock = 20 });
            products.Add(new Product { Id = 3, Title = "Juice", Price = 180, Stock = 20 });
            products.Add(new Product { Id = 4, Title = "Chicken Soup", Price = 180, Stock = 15 });

            //set coins
            List<Coin> coins = new List<Coin>();
            coins.Add(new Coin { Cents = 10, Quantity = 100});
            coins.Add(new Coin { Cents = 20, Quantity = 100});
            coins.Add(new Coin { Cents = 50, Quantity = 100});
            coins.Add(new Coin { Cents = 100, Quantity = 100});

            Machine machine = new Machine(products,coins);

            return machine;
        } 

   }
}
