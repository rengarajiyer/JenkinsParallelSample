using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trending;

namespace MSTest
{
    [TestClass]                     
    public class MsTestExample1     
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

        /// <summary>
        /// This is to show you how a method look when it fails
        /// </summary>
        [TestMethod]
        public void Test_Trending_MSTest_FAIL1() 
        {
            var result = TrendingRunner.WhatsTrending(1);
            Assert.AreEqual("nope", result);
        }
    }
}
