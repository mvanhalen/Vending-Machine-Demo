using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Logic;
namespace UnitTests
{
    [TestClass]
    public class MachineUnitTest
    {

        Machine machine = Machine.StartMachine();

        [TestMethod]
        public void TestMachineBaseProducts()
        {

            Assert.AreEqual(machine.Products.Count, 4); 

        }
        [TestMethod]
        public void TestMachineBaseCoins()
        {
            Assert.AreEqual(machine.Coins.Count, 4);


        }

        [TestMethod]
        public void InsertCoinsCount()
        {

            machine.CoinInsert(new Coin { Cents = 100, Quantity = 1, TotalCents = 100 });
            machine.CoinInsert(new Coin { Cents = 50, Quantity = 1, TotalCents = 50 });
            machine.CoinInsert(new Coin { Cents = 50, Quantity = 1, TotalCents = 50 });
            machine.CoinInsert(new Coin { Cents = 10, Quantity = 1, TotalCents = 10 });

            Assert.AreEqual(machine.CustomerCoinsValue(), 210);
            //   Assert.AreEqual(machine.Coins.Count, 4);


        }

        [TestMethod]
        public void CheckSale()
        {

            
            Assert.AreEqual(machine.CanMakeSale(1), true);
            //   Assert.AreEqual(machine.Coins.Count, 4);


        }


    }
}
