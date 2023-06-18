using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF2ItemsPricer.TF2Price.Tests
{
    [TestClass()]
    public class SKUTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            var sku_str = new string[]{
                "970;11;KT-2",
                "663;6"
            };

            var sku_exp = new SKU[]
            {
                new SKU(){Defindex = 970, Quality = 11, killstreak = 2},
                new SKU(){Defindex = 663, Quality = 6}
            };

            for(int i = 0; i < sku_str.Length;i++)
            {
                var parsed = SKU.Parse(sku_str[i]);
                if (!parsed.Equals(sku_exp[i])) Assert.Fail();
            }


            Assert.IsTrue(true);
        }
    }
}