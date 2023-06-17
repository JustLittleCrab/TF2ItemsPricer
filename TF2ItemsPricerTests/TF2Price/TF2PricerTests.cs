using Microsoft.VisualStudio.TestTools.UnitTesting;
using TF2ItemsPricer.TF2Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer.TF2Price.Tests
{
    [TestClass()]
    public class TF2PricerTests
    {
        [TestMethod()]
        public void GetPriceTest()
        {
            TF2Pricer pricer = new TF2Pricer();

            var sku = "5021;6";

            var resp = pricer.GetPrice(SKU.Parse(sku)).Result;

            Assert.IsNotNull(resp);
        }
    }
}