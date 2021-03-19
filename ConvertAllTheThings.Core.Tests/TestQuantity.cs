using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core;
using static ConvertAllTheThings.Core.Quantity;

namespace ConvertAllTheThings.Core.Tests
{
    [TestClass]
    public class TestQuantity
    {
        /*  Empty operations
         *  define basequantities
         *  *, /, pow
         *  GetFromBaseCOmposotions
         *  Correct fundamentalunits
         */

        static readonly Prefix s_testPrefix = new(2, "TestPrefix");
        static readonly BaseQuantity s_baseQuantity1;
        static readonly BaseQuantity s_baseQuantity2;


        static TestQuantity()
        {
            Global.InitializeAssembly();
            s_baseQuantity1 = BaseQuantity.DefineNewBaseQuantity(
                "Base1",
                "Fu1");

            s_baseQuantity2 = BaseQuantity.DefineNewBaseQuantity(
                "Base2",
                "Fu2",
                s_testPrefix);
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            //var quantitiesToDispose = from quantity in CompositionAndQuantitiesDictionary.Values
            //                          where quantity != Empty
            //                          select quantity;

            //while (quantitiesToDispose.Any())
            //    quantitiesToDispose.First().Dispose();
            MaybeNamed.ClearAll();
        }

        [TestMethod]
        public void TestMultiplication()
        {

        }
    }
}
