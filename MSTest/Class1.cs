using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trending;

namespace MSTest
{
    [TestClass]
    public class Class1
    {
        [TestMethod]
        public void Test_Trending_MSTest_OK1()
        {
            var result = TrendingRunner.WhatsTrending(1);
            Assert.AreEqual("Paul Walker", result);

            result = TrendingRunner.WhatsTrending(2);
            Assert.AreEqual("Cory Monteith", result);

            result = TrendingRunner.WhatsTrending(2);
            Assert.AreEqual("Cory Monteith", result);

            result = TrendingRunner.WhatsTrending(3);
            Assert.AreEqual("RoyalBaby", result);
        }
    }
}
