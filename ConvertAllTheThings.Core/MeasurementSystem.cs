using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public class MeasurementSystem : MaybeNamed, INamed
    {
        public static MeasurementSystem? Current { get; set; } = null;

        private readonly Dictionary<Quantity, IUnit> _quantities_units = new();

        static MeasurementSystem()
        {
            MaybeNamed.AddTypeToDictionary<MeasurementSystem>();
        }

        public MeasurementSystem(string name)
            : base(name)
        {
        }

        public IUnit? GetUnit(Quantity quantity)
        {
            if (_quantities_units.TryGetValue(quantity, out var res))
                return res;

            return null;
        }

        public void SetQuantityUnitPair(Quantity quantity, IUnit unit)
        {
            if (unit.Quantity != quantity)
            {
                throw new InvalidOperationException($"IUnit {unit} has quantity " +
                    $"{unit.Quantity} instead of {quantity}.");
            }

            _quantities_units[quantity] = unit;
        }

        public void SetQuantityUnitPairs(params KeyValuePair<Quantity, IUnit>[] pairs)
        {
            foreach (var pair in pairs)
                SetQuantityUnitPair(pair.Key, pair.Value);
        }

        public bool RemoveQuantity(Quantity quantity)
        {
            return _quantities_units.Remove(quantity);
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }

        protected override void DisposeBody(bool disposeDependents)
        {
            if (Current == this)
                Current = null;

            base.DisposeBody(disposeDependents);
        }
    }
}
