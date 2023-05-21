// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Linq;
using Convertal.Core.Extensions;

namespace Convertal.Core;

//public record PrefixedUnitProto(
//    string Name,
//    string? Symbol,
//    string Quantity,
//    decimal FundamentalMultiplier,
//    decimal FundamentalOffset)
//    : ScalarUnitProto(Name, Symbol, Quantity, FundamentalMultiplier, FundamentalOffset, null);
public record PrefixedUnitProto(
    string Prefix,
    string Unit)
    : MaybeNamedProto(null, null);

public abstract class PrefixedUnit : IUnit, INamed
{
    private bool _disposedValue;
    public abstract bool IsVector { get; }

    public virtual Quantity Quantity => Unit.Quantity;

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

    public virtual Unit Unit { get; }

    public Prefix Prefix { get; }

    public virtual NamedComposition<IUnit> UnitComposition { get; }

    public override string ToString()
    {
        return Name!;
    }

    public string ToStringSymbol()
    {
        return Prefix.ToStringSymbol() + "_" + Unit.ToStringSymbol();
    }

    public Database Database => Unit.Database;

    protected PrefixedUnit(Unit unit, Prefix prefix)
    {
        if (unit.Name is null)
            throw new ArgumentNullException(
                nameof(unit),
                "Unit in PrefixedUnit must have a name.");

        Unit = unit;
        Prefix = prefix;
        Database.AddToPrefixedUnitsList(this);

        // TODO reevaluate. Does this make sense?
        // Makes OtherUnitComposition always null
        UnitComposition = NamedComposition<IUnit>.Make(this);
    }

    public virtual PrefixedUnitProto ToProto()
    {
        return new(Prefix.Name!, Unit.ToString());
        //return new(Name!, Symbol, Quantity.ToString(), FundamentalMultiplier, 0);
    }
    MaybeNamedProto IMaybeNamed.ToProto() => ToProto();

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
