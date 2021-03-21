using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public sealed class EmptyQuantity : Quantity
    {
        public override EmptyUnit FundamentalUnit => EmptyUnit.Empty;

        public override BaseComposition<BaseQuantity> BaseQuantityComposition => BaseComposition<BaseQuantity>.Empty;

        public static new readonly EmptyQuantity Empty = new();

        private EmptyQuantity()
            : base(null)
        {
            Init();
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }

        protected override void Dispose(bool disposing)
        {
            // The EmptyQuantity cannot be disposed
            return;
        }
    }
}
