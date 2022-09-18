// Created by Richard Melito and licensed to you under The Clear BSD License.

namespace ConvertAllTheThings.Core;

public class PrefixedDerivedUnit : PrefixedUnit, IDerivedUnit
{
    public new DerivedUnit Unit => (DerivedUnit)base.Unit;

    internal PrefixedDerivedUnit(Database database, DerivedUnit unit, Prefix prefix)
        : base(database, unit, prefix)
    {

    }
}
