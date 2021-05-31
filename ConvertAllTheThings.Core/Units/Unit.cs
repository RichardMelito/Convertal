using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecimalMath;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public abstract class Unit : MaybeNamed, IUnit
    {
        private bool _disposed = false;

        public static EmptyUnit Empty => EmptyUnit.Empty;

        public Quantity Quantity { get; }

        public decimal FundamentalMultiplier { get; }
        public decimal FundamentalOffset { get; }
        public NamedComposition<IUnit> UnitComposition { get; protected set; }
        public NamedComposition<IUnit> UC => UnitComposition;   // just shorthand

        static Unit()
        {
            AddTypeToDictionary<Unit>();
        }

        internal static void InitializeClass() { }

        // only to be called when defining fundamental units for new
        // quantities, and thus offset will always be 0
        protected Unit(
            string? name, 
            Quantity quantity, 
            decimal fundamentalMultiplier, 
            NamedComposition<IUnit>? composition = null,
            string? symbol = null)
            : base(name, symbol)
        {
            Quantity = quantity;
            FundamentalMultiplier = fundamentalMultiplier;
            FundamentalOffset = 0;
            UnitComposition = composition ?? new(this);
        }

        protected Unit(
            string? name, 
            IUnit otherUnit, 
            decimal multiplier, 
            decimal offset, 
            string? symbol)
            : base(name, symbol)
        {
            Quantity = otherUnit.Quantity;
            FundamentalMultiplier = otherUnit.FundamentalMultiplier * multiplier;
            FundamentalOffset = (otherUnit.FundamentalOffset / multiplier) + offset;
            UnitComposition = new(this);
        }

        // for defining from a chain of operations
        protected Unit(string name, NamedComposition<IUnit> composition)
            : base(name)
        {
            // TODO: notify user that offsets will be ignored
            //var offsetQuery =
            //    from baseUnit in composition.Composition.Keys
            //    where baseUnit.FundamentalOffset != 0m
            //    select baseUnit;

            UnitComposition = composition;
            Quantity = Quantity.GetFromBaseComposition(UnitComposition);
            FundamentalMultiplier = 1m;
            FundamentalOffset = 0;
            foreach (var (unit, power) in UnitComposition.Composition)
            {
                var multiplier = DecimalEx.Pow(unit.FundamentalMultiplier, power);
                FundamentalMultiplier *= multiplier;
            }
        }

        public static NamedComposition<IUnit> Multiply(params IUnit[] units)
        {
            return MultiplyOrDivide(true, units);
        }

        public static NamedComposition<IUnit> Divide(params IUnit[] units)
        {
            return MultiplyOrDivide(false, units);
        }

        public static NamedComposition<IUnit> MultiplyOrDivide(bool multiplication, params IUnit[] units)
        {
            var res = units[0].UnitComposition;
            for (int i = 1; i < units.Length; ++i)
                res = NamedComposition<IUnit>.MultiplyOrDivide(res, units[i].UnitComposition, multiplication);

            return res;
        }

        public static Unit DefineFromComposition(string name, NamedComposition<IUnit> composition)
        {
            var quantity = Quantity.GetFromBaseComposition(composition);
            if (quantity is BaseQuantity)
                return new BaseUnit(name, composition);
            else
                return new DerivedUnit(name, composition);
        }

        public Term ConvertTo(decimal magnitudeOfThis, IUnit resultingIUnit)
        {
            return IUnit.ConvertTo(this, magnitudeOfThis, resultingIUnit);
        }

        public Term ConvertToFundamental(decimal magnitudeOfThis)
        {
            return IUnit.ConvertToFundamental(this, magnitudeOfThis);
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            var res = IUnit.GetAllDependents(this, ref toIgnore).AsEnumerable();

            var prefixedUnitsWithThis =
                from prefixedUnit in PrefixedUnit.PrefixedUnits
                where prefixedUnit.Unit == this
                select prefixedUnit;

            res = res.Union(prefixedUnitsWithThis);
            foreach (var prefixedUnit in prefixedUnitsWithThis.Except(toIgnore))
                res = res.Union(prefixedUnit.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(this);
            return res.SortByTypeAndName();
        }

        public override string ToString()
        {
            return MaybeName ?? UnitComposition!.ToString();
        }

        public bool Equals(IUnit? other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Quantity, FundamentalMultiplier);
        }

        protected override void DisposeBody(bool disposeDependents)
        {
            if (_disposed)
                return;

            if (((IUnit)this).IsFundamental && !Quantity.Disposed)
                throw new InvalidOperationException($"Cannot dispose of" +
                    $" fundamental unit {this} without first disposing of " +
                    $"quantity {Quantity}.");

            var allSystems = GetAllMaybeNameds<MeasurementSystem>().Cast<MeasurementSystem>();
            foreach (var system in allSystems)
                system.RemoveUnit(this);

            _disposed = true;
            base.DisposeBody(disposeDependents);
        }
    }
}
