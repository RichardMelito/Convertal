using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public abstract class Quantity : Named
    {
        /*  There will never be multiple quantities for something in the same 
         *  way there are multiple units for a quantity. So there's F, C, K, etc. 
         *  BaseUnits for Temperature, but there's only 1 Temperature.
         *  
         *  FundamentalUnit
         *  BaseQuantityComposition
         *  
         *  Name - Unnamed namespace? HasName? Temporary names?
         *      - temporaries have Unnamed namespace and fullname of their composition
         */

        private bool _disposed = false;
        private static readonly Dictionary<BaseComposition<BaseQuantity>, Quantity> s_compositions_quantities = new();

        public abstract Unit FundamentalUnit { get; }
        public BaseComposition<BaseQuantity> BaseQuantityComposition { get; }

        protected Quantity(
            string name, 
            string nameSpace, 
            BaseComposition<BaseQuantity> composition)
            : base(name, nameSpace)
        {
            BaseQuantityComposition = composition;
        }

        public static Quantity operator* (Quantity lhs, Quantity rhs)
        {
            var resultingComposition = lhs.BaseQuantityComposition * rhs.BaseQuantityComposition;
            if (s_compositions_quantities.TryGetValue(resultingComposition, out var associatedQuantity))
                return associatedQuantity;

            
        }


        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;


            if (!s_compositions_quantities.Remove(BaseQuantityComposition))
                throw new ApplicationException(
                    $"Could not remove Quantity {FullName} with composition " +
                    $"{BaseQuantityComposition} from static dictionary.");

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
