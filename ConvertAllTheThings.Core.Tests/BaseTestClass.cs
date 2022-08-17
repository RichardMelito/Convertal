using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core;

namespace ConvertAllTheThings.Core.Tests
{
    public abstract class BaseTestClass
    {
        public const decimal DELTA = 0.000000001m;
        protected Database Database { get; } = new();
    }
}
