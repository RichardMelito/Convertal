using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    // should maybe inherit from Unit?
    public sealed class EmptyUnit : IUnit
    {
        public static readonly EmptyUnit Empty = new();

        public decimal FundamentalMultiplier => 1m;
        public decimal FundamentalOffset => 0;

        public Quantity Quantity => Quantity.Empty;

        public NamedComposition<IUnit> UnitComposition => NamedComposition<IUnit>.Empty;

        public string? MaybeName => null;

        public string? MaybeSymbol => null;

        private EmptyUnit()
        {

        }

        public override string ToString()
        {
            return "";
        }

        public string ToStringSymbol()
        {
            return "";
        }

        public bool Equals(IUnit? other)
        {
            return base.Equals(other);
        }

        void IMaybeNamed.DisposeThisAndDependents(bool disposeDependents)
        {
            // The EmptyUnit cannot be disposed
            return;
        }

        public void Dispose()
        {
            // The EmptyUnit cannot be disposed
            return;
        }

        public IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }
    }
}
