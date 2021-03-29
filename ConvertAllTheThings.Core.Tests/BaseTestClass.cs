using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core;

namespace ConvertAllTheThings.Core.Tests
{
    [TestClass]
    public abstract class BaseTestClass
    {
        public const decimal DELTA = 0.000000001m;

        static BaseTestClass()
        {
            Global.InitializeAssembly();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            MaybeNamed.ClearAll();
        }
    }
}
