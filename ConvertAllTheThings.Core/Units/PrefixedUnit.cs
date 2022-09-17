// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Linq;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core;

public record PrefixedUnitProto(
    string Name,
    string? Symbol,
    string Quantity,
    decimal FundamentalMultiplier,
    decimal FundamentalOffset)
    : UnitProto(Name, Symbol, Quantity, FundamentalMultiplier, FundamentalOffset, null);

public abstract class PrefixedUnit : IUnit, INamed
{
    private bool _disposedValue;

    public Quantity Quantity => Unit.Quantity;

    public string? Name => Prefix.Name! + "_" + Unit.Name!;

    public string? Symbol
    {
        get
        {
            if (Prefix.Symbol is null || Unit.Symbol is null)
                return null;

            return Prefix.Symbol + "_" + Unit.Symbol;
        }
    }

    public decimal FundamentalMultiplier => Unit.FundamentalMultiplier * Prefix.Multiplier;
    public decimal FundamentalOffset => Unit.FundamentalOffset / Prefix.Multiplier;

    public Unit Unit { get; private set; }

    public Prefix Prefix { get; }

    public NamedComposition<IUnit> UnitComposition { get; }

    public override string ToString()
    {
        return Name!;
    }

    public string ToStringSymbol()
    {
        return Prefix.ToStringSymbol() + "_" + Unit.ToStringSymbol();
    }

    public Database Database { get; }

    protected PrefixedUnit(Database database, Unit unit, Prefix prefix)
    {
        Database = database;

        if (unit.Name is null)
            throw new ArgumentNullException(
                nameof(unit),
                "Unit in PrefixedUnit must have a name.");

        Prefix = prefix;
        Database.AddToPrefixedUnitsList(this);
        Unit = unit;

        // TODO reevaluate. Does this make sense?
        // Makes OtherUnitComposition always null
        UnitComposition = new(this);
    }

    public PrefixedUnitProto ToProto()
    {
        return new(Name!, Symbol, Quantity.ToString(), FundamentalMultiplier, FundamentalOffset);
    }
    MaybeNamedProto IMaybeNamed.ToProto() => ToProto();


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
            if (!Database.RemoveFromPrefixedUnitsList(this))
                throw new ApplicationException($"Could not remove {this} from " +
                    $"the PrefixedUnit dictionary.");

            if (disposeDependents)
            {
                var toIgnore = this.Encapsulate().Cast<IMaybeNamed>();
                var dependents = ((IUnit)this).GetAllDependents(ref toIgnore).ToArray();
                foreach (var dependent in dependents)
                    dependent.DisposeThisAndDependents(false);
            }


            var allSystems = Database.GetAllMaybeNameds<MeasurementSystem>();
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
