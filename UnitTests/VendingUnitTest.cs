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
            //insert 160
            machine.CoinInsertCustomer(new Coin { Cents = 100, Quantity = 1 });
            machine.CoinInsertCustomer(new Coin { Cents = 50, Quantity = 1 });
            machine.CoinInsertCustomer(new Coin { Cents = 10, Quantity = 1 });

            Assert.AreEqual(machine.CustomerCoinsValue(), 160);
            


        }


        [TestMethod]
        public void CheckSaleNoMoney()
        {

          
            //cannot buy product of 1.80 without credit
            Assert.AreEqual(machine.CanMakeSale(2), false);
        }

        


        [TestMethod]
        public void CheckSale()
        {
            //insert 200 1x 1 2x 0.50
            machine.CoinInsertCustomer(new Coin { Cents = 100, Quantity = 1 });
            machine.CoinInsertCustomer(new Coin { Cents = 50, Quantity = 2 });
           
            Assert.AreEqual(machine.CanMakeSale(1), true);
        }

        [TestMethod]
        public void MakeSaleCheckChange()
        {
            machine.CoinInsertCustomer(new Coin { Cents = 100, Quantity = 1 });
            machine.CoinInsertCustomer(new Coin { Cents = 50, Quantity = 2 });

            //200  inserted. Can make sale of 180. Change should be 1 coin of 20
            Assert.AreEqual(machine.MakeSale(2)[0].Cents, 20 );

        }



    }
}
