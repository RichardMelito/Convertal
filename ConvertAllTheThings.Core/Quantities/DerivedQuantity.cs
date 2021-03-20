using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class DerivedQuantity : Quantity, IDerived
    {
        public override BaseComposition<BaseQuantity> BaseQuantityComposition { get; }

        public override IUnit FundamentalUnit { get; }

        /// <summary>
        /// To be called only from <see cref="Quantity.GetFromBaseComposition(BaseComposition{BaseQuantity})"/>
        /// </summary>
        /// <param name="composition"></param>
        internal DerivedQuantity(BaseComposition<BaseQuantity> composition)
            : base(null)
        {
            BaseQuantityComposition = composition;
            FundamentalUnit = new DerivedUnit(this);
            Init();
        }
    }
}
