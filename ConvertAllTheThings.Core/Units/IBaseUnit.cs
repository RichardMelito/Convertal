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


        static IEnumerable<IDerivedUnit> GetAllIDerivedUnitsComposedOf(IBaseUnit baseUnit)
        {
            var allUnits = MaybeNamed.GetAllMaybeNameds<Unit>().Cast<Unit>();

            IEnumerable<IDerivedUnit> unitsComposedOfGiven = 
                from unit in allUnits
                where unit is DerivedUnit &&
                unit.MaybeBaseUnitComposition is not null &&
                unit.MaybeBaseUnitComposition.Composition.ContainsKey(baseUnit)
                select (DerivedUnit)unit;

            IEnumerable<IDerivedUnit> prefixedUnitsComposedOfGiven =
                from prefixedUnit in PrefixedUnit.PrefixedUnits
                where prefixedUnit is PrefixedDerivedUnit &&
                prefixedUnit.MaybeBaseUnitComposition is not null &&
                prefixedUnit.MaybeBaseUnitComposition.Composition.ContainsKey(baseUnit)
                select (PrefixedDerivedUnit)prefixedUnit;

            return unitsComposedOfGiven.Union(prefixedUnitsComposedOfGiven);
        }

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
