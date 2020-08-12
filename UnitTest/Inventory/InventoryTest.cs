using System.Collections.Generic;
using System.Linq;
using Core.Inventory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Inventory
{
    [TestClass]
    public class InventoryTest
    {

        [TestMethod]
        public void Test_AddTyre()
        {
            int savedId = Product.GetNextAvailableId();
            Assert.IsTrue(Tyre.AddNewTyre(new Tyre
                (savedId, "215/55X20 DSI JAPAN TYRE", "DSI", "215/55X20", "JAPAN", 1000, 1)));
        }

        [TestMethod]
        public void Test_AddAlloyWheel()
        {
            int savedId = Product.GetNextAvailableId();
            Assert.IsTrue(AlloyWheel.AddNewAlloyWheel(new AlloyWheel
                (savedId, "215/55X20 MAX ALLOYWHEEL", 1, 1000, "MAX", "215/55X20")));
        }

        [TestMethod]
        public void Test_AddBattery()
        {
            int savedId = Product.GetNextAvailableId();
            Assert.IsTrue(Battery.AddNewBattery(new Battery
                (savedId, "AMARON 9V", 2, 4000, "AMARON")));
        }

        [TestMethod]
        public void Test_GetTyre()
        {
            int savedId = Product.GetNextAvailableId();
            Tyre testTyre = new Tyre
                (savedId, "215/55X21 DSI JAPAN TYRE", "DSI", "215/55X21", "JAPAN", 1000, 1);
            Tyre.AddNewTyre(testTyre);

            Assert.AreEqual
                (testTyre.Name, Tyre.GetTyres(id : savedId.ToString()).ElementAt(0).Name);
            Assert.AreEqual
                (testTyre.Name, Tyre.GetTyres(name: testTyre.Name).ElementAt(0).Name);
        }

        [TestMethod]
        public void Test_GetAlloyWheel()
        {
            int savedId = Product.GetNextAvailableId();
            AlloyWheel testAlloyWheel = new AlloyWheel
                (savedId, "215/55X21 MAX ALLOYWHEEL", 1, 1000, "MAX", "215/55X20");
            AlloyWheel.AddNewAlloyWheel(testAlloyWheel);

            Assert.AreEqual
                (testAlloyWheel.Name, AlloyWheel.GetAlloyWheels(id: savedId.ToString()).ElementAt(0).Name);
            Assert.AreEqual
                (testAlloyWheel.Name, AlloyWheel.GetAlloyWheels(name: testAlloyWheel.Name).ElementAt(0).Name);
        }

        [TestMethod]
        public void Test_GetBattery()
        {
            int savedId = Product.GetNextAvailableId();
            Battery testBattery = new Battery(savedId, "AMARON 9V EXTRA", 2, 4000, "AMARON");
            Battery.AddNewBattery(testBattery);

            Assert.AreEqual
                (testBattery.Name,Battery.GetBatteries(id: savedId.ToString()).ElementAt(0).Name);
            Assert.AreEqual
                (testBattery.Name,Battery.GetBatteries(name: testBattery.Name).ElementAt(0).Name);
        }

        [TestMethod]
        public void Test_DupplicateTyre()
        {
            int savedId = Product.GetNextAvailableId();
            Assert.IsTrue(Tyre.AddNewTyre(new Tyre
                (savedId, "215/55X22 DSI JAPAN TYRE", "DSI", "215/55X22", "JAPAN", 1000, 1)));
            Assert.IsFalse(Tyre.AddNewTyre(new Tyre
                (savedId, "215/55X22 DSI JAPAN TYRE", "DSI", "215/55X22", "JAPAN", 1000, 1)));
        }

        [TestMethod]
        public void Test_DupplicateAlloyWheel()
        {
            int savedId = Product.GetNextAvailableId();
            Assert.IsTrue(AlloyWheel.AddNewAlloyWheel(new AlloyWheel
                (savedId, "215/55X22 MAX ALLOYWHEEL", 1, 1000, "MAX", "215/55X22")));
            Assert.IsFalse(AlloyWheel.AddNewAlloyWheel(new AlloyWheel
                 (Product.GetNextAvailableId(), "215/55X22 MAX ALLOYWHEEL", 1, 1000, "MAX", "215/55X22")));
        }

        [TestMethod]
        public void Test_DupplicateBattery()
        {
            int savedId = Product.GetNextAvailableId();
            Assert.IsTrue(Battery.AddNewBattery(new Battery
                (savedId, "AMARON 9V EXTRA LIFE", 2, 4000, "AMARON")));
            Assert.IsFalse(Battery.AddNewBattery(new Battery
                (Product.GetNextAvailableId(), "AMARON 9V EXTRA LIFE", 2, 4000, "AMARON")));
        }

    }
}
