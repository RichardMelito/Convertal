using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public abstract class PrefixedUnit : IUnit, INamed
    {
        private bool _disposedValue;

        public Quantity Quantity => Unit.Quantity;

        public string? MaybeName => Prefix.MaybeName! + "_" + Unit.MaybeName!;

        public decimal FundamentalMultiplier => Unit.FundamentalMultiplier * Prefix.Multiplier;
        public abstract Unit Unit { get; }
        public Prefix Prefix { get; }

        public BaseComposition<IBaseUnit>? MaybeBaseUnitComposition { get; protected set; }

        public override string ToString()
        {
            return MaybeName!;
        }

        protected PrefixedUnit(Unit unit, Prefix prefix)
        {
            if (unit.MaybeName is null)
                throw new ArgumentNullException(
                    nameof(unit), 
                    "Unit in PrefixedUnit must have a name.");

            Prefix = prefix;
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

        #region IDisposable boilerplate
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
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
