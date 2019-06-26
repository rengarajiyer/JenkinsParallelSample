using System;
using FluentAssertions;
using NUnit.Framework;
using Trending;

namespace NUnitTest
{

    public class NUnitTest
    {
        [TestCase]                 
        public void Test_Trending_NUnit_Fluent_OK() 
        {
            Console.Out.WriteLine("Hi from Nunit");

            var result = TrendingRunner.WhatsTrending(1);
            result.Should().Be("Paul Walker");
        }

        [TestCase]
        public void Test_Trending_NUnit_Fluent_FAIL()
        {
            Console.Out.WriteLine("Hi from Nunit");

            var result = TrendingRunner.WhatsTrending(1);
            result.Should().Be("Paul Talker"); // This should fail, typo.
        }

                        
        [TestCase(1, Result = "Paul Walker")]
        [TestCase(2, Result = "Cory b")]        // This should fail, typo.
        [TestCase(3, Result = "RoyalBaby")]
        public string Test_Trending_NUnit(int anIndex)
        {
            var result = TrendingRunner.WhatsTrending(anIndex);
            Console.Out.WriteLine("Call \t-> \tresult :\r\n  {0} \t-> \t\"{1}\""
                                , anIndex
                                , result );
            return result;
        }
        

    }
}
