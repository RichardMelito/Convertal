using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class DerivedQuantity : Quantity, IDerived
    {
        public override NamedComposition<BaseQuantity> BaseQuantityComposition { get; }

        public override IUnit FundamentalUnit { get; }

        /// <summary>
        /// To be called only from <see cref="Quantity.GetFromBaseComposition(NamedComposition{BaseQuantity})"/>
        /// </summary>
        /// <param name="composition"></param>
        internal DerivedQuantity(Database database, NamedComposition<BaseQuantity> composition)
            : base(database, null, null)
        {
            BaseQuantityComposition = composition;
            FundamentalUnit = new DerivedUnit(database, this);
            Init();
        }
    }
}
