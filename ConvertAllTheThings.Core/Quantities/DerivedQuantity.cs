using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class DerivedQuantity : Quantity
    {
        public override BaseComposition<BaseQuantity> BaseQuantityComposition { get; }

        public override IUnit FundamentalUnit => throw new NotImplementedException();

        /// <summary>
        /// To be called only from <see cref="Quantity.GetFromBaseComposition(BaseComposition{BaseQuantity})"/>
        /// </summary>
        /// <param name="composition"></param>
        internal DerivedQuantity(BaseComposition<BaseQuantity> composition)
            : base(null)
        {
            BaseQuantityComposition = composition;
            Init();
        }
    }
}
