using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core.Tests
{
    public static class MoreAssertions
    {
        public static void AssertAreEqual(decimal lhs, decimal rhs, decimal delta = 0m)
        {
            var diff = Math.Abs(lhs - rhs);
            Assert.True(diff < delta);
        }
    }
}
