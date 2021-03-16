using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    // should maybe inherit from Unit?
    public class EmptyUnit : IUnit
    {
        public static readonly EmptyUnit Empty = new();

        public decimal FundamentalMultiplier => 1m;

        public Quantity Quantity => Quantity.Empty;

        public BaseComposition<IBaseUnit>? MaybeBaseUnitComposition => BaseComposition<IBaseUnit>.Empty;

        public string? MaybeName => null;

        private EmptyUnit()
        {

        }
    }
}
