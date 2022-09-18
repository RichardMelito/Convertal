// Created by Richard Melito and licensed to you under The Clear BSD License.

using Xunit;

namespace ConvertAllTheThings.Core.Tests;

public static class MoreAssertions
{
    public static void AssertAreEqual(decimal lhs, decimal rhs, decimal delta = 0m)
    {
        var diff = Math.Abs(lhs - rhs);
        Assert.True(diff < delta);
    }
}
