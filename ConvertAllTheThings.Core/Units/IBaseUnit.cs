using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public interface IBaseUnit : IUnit, IBase, IEquatable<IBaseUnit>
        // TODO interface hierarchy for IEquatable and IComparable
    {
        BaseQuantity BaseQuantity => (BaseQuantity)Quantity;

        static IEnumerable<IDerivedUnit> GetAllIDerivedUnitsComposedOf(IBaseUnit baseUnit)
        {
            var allUnits = MaybeNamed.GetAllMaybeNameds<Unit>().Cast<Unit>();

            IEnumerable<IDerivedUnit> unitsComposedOfGiven = 
                from unit in allUnits
                where unit is DerivedUnit &&
                unit.UnitComposition is not null &&
                unit.UnitComposition.Composition.ContainsKey(baseUnit)
                select (DerivedUnit)unit;

            IEnumerable<IDerivedUnit> prefixedUnitsComposedOfGiven =
                from prefixedUnit in PrefixedUnit.PrefixedUnits
                where prefixedUnit is PrefixedDerivedUnit &&
                prefixedUnit.UnitComposition is not null &&
                prefixedUnit.UnitComposition.Composition.ContainsKey(baseUnit)
                select (PrefixedDerivedUnit)prefixedUnit;

            return unitsComposedOfGiven.Union(prefixedUnitsComposedOfGiven);
        }

        bool IEquatable<IBaseUnit>.Equals(IBaseUnit? other)
        {
            return ReferenceEquals(this, other);
        }
    }
}
