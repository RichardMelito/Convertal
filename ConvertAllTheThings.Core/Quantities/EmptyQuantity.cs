using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override IEnumerable<IMaybeNamed> GetAllDependents()
        {
            return Array.Empty<IMaybeNamed>();
        }

        protected override void Dispose(bool disposing)
        {
            // The EmptyQuantity cannot be disposed
            return;
        }
    }
}
