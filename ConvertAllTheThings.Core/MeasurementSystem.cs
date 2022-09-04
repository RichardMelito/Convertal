using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;
using System.Text.Json.Serialization;
using System.Collections;

namespace ConvertAllTheThings.Core
{
    public class MeasurementSystem : MaybeNamed, INamed
    {
        // TODO
        public static MeasurementSystem? Current { get; set; } = null;

        private readonly Dictionary<Quantity, IUnit> _quantities_units = new();

        [JsonConverter(typeof(JsonConverters.ConverterForDictionaries))]
        public IReadOnlyDictionary<Quantity, IUnit> Dictionary { get; } 

        internal MeasurementSystem(Database database, string name)
            : base(database, name)
        {
            Dictionary = _quantities_units.AsReadOnly();
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

        public bool RemoveUnit(IUnit unit)
        {
            if (_quantities_units.ContainsValue(unit))
                return _quantities_units.Remove(unit.Quantity);

            return false;
        }

        public bool RemoveQuantity(Quantity quantity)
        {
            return _quantities_units.Remove(quantity);
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            // nothing should depend on a MeasurementSystem
            toIgnore = toIgnore.UnionAppend(this);
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }

        protected override void DisposeBody(bool disposeDependents)
        {
            // nothing should depend on a MeasurementSystem

            if (Current == this)
                Current = null;

            base.DisposeBody(disposeDependents);
        }
    }
}
