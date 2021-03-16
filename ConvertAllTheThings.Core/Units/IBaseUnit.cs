using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public interface IBaseUnit : IUnit, IBase, IEquatable<IBaseUnit>, IComparable<IBaseUnit>    
        // TODO interface hierarchy for IEquatable and IComparable
    {
        BaseQuantity BaseQuantity => (BaseQuantity)Quantity;
        BaseComposition<IBaseUnit> BaseUnitComposition => MaybeBaseUnitComposition!;


        bool IEquatable<IBaseUnit>.Equals(IBaseUnit? other)
        {
            return ReferenceEquals(this, other);
        }

        int IComparable<IBaseUnit>.CompareTo(IBaseUnit? other)
        {
            return MaybeNamed.MaybeNameComparer.PerformCompare(this, other);
        }
    }
}
