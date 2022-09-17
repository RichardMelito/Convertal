namespace ConvertAllTheThings.Core
{
    public class PrefixedDerivedUnit : PrefixedUnit, IDerivedUnit
    {
        public new DerivedUnit Unit => (DerivedUnit)base.Unit;

        internal PrefixedDerivedUnit(Database database, DerivedUnit unit, Prefix prefix)
            : base(database, unit, prefix)
        {

        }
    }
}
