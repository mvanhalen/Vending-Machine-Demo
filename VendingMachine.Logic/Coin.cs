using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Logic
{
    /// <summary>
    /// The coin object which can represent multipe coins of the same value in cents. Totalcents is accumulated
    /// </summary>
    public class Coin
    {
        public int Cents { get; set;}
        public int Quantity { get; set;}
        public int TotalCents { get; set; }
    }
}
