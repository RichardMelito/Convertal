using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecimalMath;
using ConvertAllTheThings.Core.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConvertAllTheThings.Core
{
    public record UnitProto(
        string? Name, 
        string? Symbol, 
        [property: JsonPropertyOrder(2)] string Quantity,
        [property: JsonPropertyOrder(3)] decimal FundamentalMultiplier,
        [property: JsonPropertyOrder(4), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] decimal FundamentalOffset,
        [property: JsonPropertyOrder(5)] ValueEqualityDictionary<string, decimal>? OtherUnitComposition) 
        : MaybeNamedProto(Name, Symbol);

    public abstract class Unit : MaybeNamed, IUnit
    {
        private bool _disposed = false;
        private NamedComposition<IUnit>? _unitComposition;

        public Quantity Quantity { get; }

        public decimal FundamentalMultiplier { get; }

        public decimal FundamentalOffset { get; }

        public NamedComposition<IUnit>? OtherUnitComposition => ((IUnit)this).GetOtherUnitComposition();

        public NamedComposition<IUnit> UnitComposition 
        { 
            get => _unitComposition!;
            internal set
            {
                if (_unitComposition is null || _unitComposition.IsComposedOfOne(this))
                    _unitComposition = value;
                else
                    throw new InvalidOperationException();
            }
        }


        public NamedComposition<IUnit> UC => UnitComposition;   // just shorthand. TODO delete this

        // only to be called when defining fundamental units for new
        // quantities, and thus offset will always be 0
        protected Unit(
            Database database,
            string? name,
            Quantity quantity,
            decimal fundamentalMultiplier,
            NamedComposition<IUnit>? composition = null,
            string? symbol = null)
            : base(database, name, symbol)
        {
            Quantity = quantity;
            FundamentalMultiplier = fundamentalMultiplier;
            FundamentalOffset = 0;
            composition?.ThrowIfRecursive(this);
            UnitComposition = composition ?? new(this);
        }

        protected Unit(
            Database database,
            string? name,
            IUnit otherUnit,
            decimal multiplier,
            decimal offset,
            string? symbol)
            : base(database, name, symbol)
        {
            Quantity = otherUnit.Quantity;
            FundamentalMultiplier = otherUnit.FundamentalMultiplier * multiplier;
            FundamentalOffset = (otherUnit.FundamentalOffset / multiplier) + offset;
            UnitComposition = new(this);
        }

        // for defining from a chain of operations
        protected Unit(Database database, string name, NamedComposition<IUnit> composition)
            : base(database, name)
        {
            // TODO: notify user that offsets will be ignored
            //var offsetQuery =
            //    from baseUnit in composition.Keys
            //    where baseUnit.FundamentalOffset != 0m
            //    select baseUnit;

            composition.ThrowIfRecursive(this);
            UnitComposition = composition;
            Quantity = Database.GetFromBaseComposition(UnitComposition);
            FundamentalMultiplier = 1m;
            FundamentalOffset = 0;
            foreach (var (unit, power) in UnitComposition)
            {
                var multiplier = DecimalEx.Pow(unit.FundamentalMultiplier, power);
                FundamentalMultiplier *= multiplier;
            }
        }

        // deserialization constructor
        protected Unit(Database database, string? name, string? symbol, Quantity quantity, decimal fundamentalMultiplier, decimal fundamentalOffset, NamedComposition<IUnit>? composition) 
            : base(database, name, symbol)
        {
            Quantity = quantity;
            FundamentalMultiplier = fundamentalMultiplier;
            FundamentalOffset = fundamentalOffset;
            composition?.ThrowIfRecursive(this);
            UnitComposition = composition ?? new(this);
        }

        public override UnitProto ToProto()
        {
            return new(
                Name, 
                Symbol, 
                Quantity.ToString(),
                FundamentalMultiplier, 
                FundamentalOffset, 
                OtherUnitComposition is null ? null : new(OtherUnitComposition.CompositionAsStringDictionary));
        }
        protected override Type GetDatabaseType() => typeof(Unit);

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
                from prefixedUnit in Database.PrefixedUnits
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
            return Name ?? UnitComposition!.ToString();
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

            var allSystems = Database.GetAllMaybeNameds<MeasurementSystem>();
            foreach (var system in allSystems)
                system.RemoveUnit(this);

            _disposed = true;
            base.DisposeBody(disposeDependents);
        }
    }
}
