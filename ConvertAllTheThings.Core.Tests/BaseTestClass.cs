// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace ConvertAllTheThings.Core.Tests;

public abstract class BaseTestClass
{
    public const decimal DELTA = 0.000000001m;
    protected Database Database { get; } = new();
}
