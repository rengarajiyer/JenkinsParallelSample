using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trending;

namespace MSFluent
{
    [TestClass]
    public class MsFluent
    {
        [TestMethod]
        public void Test_Trending_MSTest_Fluent_OK()
        {
            var result = TrendingRunner.WhatsTrending(1);
            result.Should().Be("Paul Walker");
        }

        [TestMethod]
        public void Test_Trending_MSTest_Fluent_Fail()
        {
            var result = TrendingRunner.WhatsTrending(1);
            result.Should().Be("Paul Talker"); // fail because of typo
        }
    }
}
