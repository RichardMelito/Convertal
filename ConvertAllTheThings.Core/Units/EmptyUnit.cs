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
        public decimal FundamentalMultiplier => 1m;
        public decimal FundamentalOffset => 0;

        public Quantity Quantity { get; }

        public NamedComposition<IUnit> UnitComposition => NamedComposition<IUnit>.Empty;

        public string? MaybeName => null;

        public string? MaybeSymbol => null;

        internal EmptyUnit(Database database)
        {
            Quantity = database.EmptyQuantity;
        }

        public override string ToString()
        {
            return "";
        }

        public string ToStringSymbol()
        {
            return "";
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
            toIgnore = toIgnore.UnionAppend(this);
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }
    }
}
