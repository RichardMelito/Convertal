using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public abstract class PrefixedUnit : IUnit, INamed
    {
        private bool _disposedValue;

        public Quantity Quantity => Unit.Quantity;

        public string? MaybeName => Prefix.MaybeName! + "_" + Unit.MaybeName!;

        public string? MaybeSymbol
        {
            get
            {
                if (Prefix.MaybeSymbol is null || Unit.MaybeSymbol is null)
                    return null;

                return Prefix.MaybeSymbol + "_" + Unit.MaybeSymbol;
            }
        }

        public decimal FundamentalMultiplier => Unit.FundamentalMultiplier * Prefix.Multiplier;
        public decimal FundamentalOffset => Unit.FundamentalOffset / Prefix.Multiplier;
        public Unit Unit { get; private set; }
        public Prefix Prefix { get; }

        public NamedComposition<IUnit> UnitComposition { get; protected set; }

        public override string ToString()
        {
            return MaybeName!;
        }

        public string ToStringSymbol()
        {
            return Prefix.ToStringSymbol() + "_" + Unit.ToStringSymbol();
        }

        private static readonly List<PrefixedUnit> s_prefixedUnits = new();
        public static readonly ReadOnlyCollection<PrefixedUnit> 
            PrefixedUnits = s_prefixedUnits.AsReadOnly();

        [JsonIgnore]
        internal Database Database { get; }

        protected PrefixedUnit(Database database, Unit unit, Prefix prefix)
        {
            Database = database;

            if (unit.MaybeName is null)
                throw new ArgumentNullException(
                    nameof(unit), 
                    "Unit in PrefixedUnit must have a name.");

            Prefix = prefix;
            s_prefixedUnits.Add(this);
            Unit = unit;

            UnitComposition = new(this);
        }

        public static PrefixedBaseUnit GetPrefixedUnit(BaseUnit unit, Prefix prefix)
        {
            return (PrefixedBaseUnit)GetPrefixedUnit((Unit)unit, prefix);
        }

        public static PrefixedDerivedUnit GetPrefixedUnit(DerivedUnit unit, Prefix prefix)
        {
            return (PrefixedDerivedUnit)GetPrefixedUnit((Unit)unit, prefix);
        }

        public static PrefixedUnit GetPrefixedUnit(Unit unit, Prefix prefix)
        {
            var existingPrefixedUnit =
                from prefixedUnit in PrefixedUnits
                where prefixedUnit.Unit == unit &&
                prefixedUnit.Prefix == prefix
                select prefixedUnit;

            switch (existingPrefixedUnit.Count())
            {
                case 0:
                    if (unit is BaseUnit baseUnit)
                        return new PrefixedBaseUnit(unit.Database, baseUnit, prefix);

                    else if (unit is DerivedUnit derivedUnit)
                        return new PrefixedDerivedUnit(unit.Database, derivedUnit, prefix);

                    else
                        throw new NotImplementedException();

                case 1:
                    return existingPrefixedUnit.First();

                default:
                    throw new ApplicationException(existingPrefixedUnit.Count().ToString());
            }
        }


        public Term ConvertTo(decimal magnitudeOfThis, IUnit resultingIUnit)
        {
            return IUnit.ConvertTo(this, magnitudeOfThis, resultingIUnit);
        }

        public Term ConvertToFundamental(decimal magnitudeOfThis)
        {
            return IUnit.ConvertToFundamental(this, magnitudeOfThis);
        }

        public bool Equals(IUnit? other)
        {
            if (other is not PrefixedUnit cast)
                return false;

            return Unit.Equals(cast.Unit) && Prefix.Equals(cast.Prefix);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PrefixedUnit);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Unit, Prefix);
        }

        void IMaybeNamed.DisposeThisAndDependents(bool disposeDependents)
        {
            DisposeBody(disposeDependents);
        }



        protected virtual void DisposeBody(bool disposeDependents)
        {
            if (!_disposedValue)
            {
                if (((IUnit)this).IsFundamental && !Quantity.Disposed)
                    throw new InvalidOperationException($"Cannot dispose of" +
                        $" fundamental unit {this} without first disposing of " +
                        $"quantity {Quantity}.");

                // TODO: dispose managed state (managed objects)
                if (!s_prefixedUnits.Remove(this))
                    throw new ApplicationException($"Could not remove {this} from " +
                        $"the PrefixedUnit dictionary.");

                if (disposeDependents)
                {
                    var toIgnore = this.Encapsulate().Cast<IMaybeNamed>();
                    var dependents = ((IUnit)this).GetAllDependents(ref toIgnore).ToArray();
                    foreach (var dependent in dependents)
                        dependent.DisposeThisAndDependents(false);
                }


                var allSystems = Database.GetAllMaybeNameds<MeasurementSystem>().Cast<MeasurementSystem>();
                foreach (var system in allSystems)
                    system.RemoveUnit(this);

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        #region IDisposable boilerplate
        protected virtual void Dispose(bool disposing, bool disposeDependents = true)
        {
            if (disposing)
                DisposeBody(disposeDependents);
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PrefixedUnit()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
