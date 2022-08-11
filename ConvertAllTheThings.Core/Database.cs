using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConvertAllTheThings.Core.Extensions;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class Database
    {
        // serialization lists
        private readonly List<BaseQuantity> _baseQuantities = new();
        private readonly List<BaseQuantity> _derivedQuantities = new();
        private readonly List<BaseUnit> _baseUnits = new();
        private readonly List<DerivedUnit> _derivedUnits = new();
        private readonly List<Prefix> _prefixes = new();
        private readonly List<PrefixedBaseUnit> _prefixedBaseUnits = new();
        private readonly List<PrefixedDerivedUnit> _prefixedDerivedUnits = new();
        private readonly List<MeasurementSystem> _measurementSystems = new();

        internal Dictionary<Type, List<MaybeNamed>> MaybeNamedsByType { get; } = new();
        internal Dictionary<NamedComposition<BaseQuantity>, Quantity> QuantitiesByComposition { get; } = new();

        public IReadOnlyDictionary<NamedComposition<BaseQuantity>, Quantity> CompositionAndQuantitiesDictionary { get; }

        /*  Move name lookup/storage stuff from MaybeNamed into here
         *  CRUD for named objects occurs here
         */

        public Database()
        {
            AddTypeToDictionary<MeasurementSystem>();
            AddTypeToDictionary<Prefix>();
            AddTypeToDictionary<Quantity>();
            AddTypeToDictionary<Unit>();

            CompositionAndQuantitiesDictionary = QuantitiesByComposition.AsReadOnly();
        }

        public IEnumerable<MaybeNamed> GetAllMaybeNameds<T>()
        {
            return MaybeNamedsByType[GetTypeWithinDictionary(typeof(T))!];
        }

        public Type? GetTypeWithinDictionary(Type type)
        {
            var originalType = type;

            while (!MaybeNamedsByType.ContainsKey(type))
            {
                if (type.BaseType is null)
                    return null;

                //if (type.BaseType is null)
                //    throw new ArgumentException($"Neither type {originalType.Name} " +
                //        $"nor any of its base types are within the name lookup dictionary.");

                type = type.BaseType;
            }

            return type;
        }

        protected void AddTypeToDictionary<T>()
            where T : MaybeNamed
        {
            MaybeNamedsByType.Add(typeof(T), new List<MaybeNamed>());
        }

        public T? FromString<T>(string str)
            where T : IMaybeNamed
        {
            var type = typeof(T);
            if (type.IsSubclassOf(typeof(MaybeNamed)))
            {
                if (TryGetFromName(str, type, out var res))
                    return res!.ForceCast<T>();

                else
                    return default;
            }

            if (type.IsSubclassOf(typeof(PrefixedUnit)))
            {
                var split = str.Split('_');
                if (split.Length != 2)
                    throw new ArgumentException(str);

                var prefix = GetFromName<Prefix>(split[0]);
                var unit = GetFromName<Unit>(split[1]);
                return PrefixedUnit.GetPrefixedUnit(unit, prefix).ForceCast<T>();
            }

            throw new ArgumentException(str);

            //T? res = default;
            //res = res switch
            //{
            //    MaybeNamed => (T)new Object(),
            //    _ => throw new Exception()
            //};

            //return res;
        }



        public bool TryGetFromName(
            string name,
            Type type,
            out MaybeNamed? namedObject,
            bool isSymbol = false)
        {
            var typeWithinDictionary = GetTypeWithinDictionary(type);
            if (typeWithinDictionary is null)
            {
                namedObject = null;
                return false;
            }

            var nameds = MaybeNamedsByType[typeWithinDictionary];
            MaybeNamed[] matches;
            if (isSymbol)
            {
                matches = (from named in nameds
                           where named.MaybeSymbol == name
                           select named).ToArray();
            }
            else
            {
                matches = (from named in nameds
                           where named.MaybeName == name
                           select named).ToArray();
            }

            if (matches.Length == 1)
            {
                namedObject = matches.First();
                return true;
            }
            else if (matches.Length == 0)
            {
                namedObject = null;
                return false;
            }
            else
            {
                throw new ApplicationException();
            }
        }

        public bool TryGetFromName<T>(
            string name,
            out T? namedObject,
            bool isSymbol = false)
            where T : MaybeNamed
        {
            TryGetFromName(name, typeof(T), out var named, isSymbol);
            namedObject = named as T;
            return namedObject is not null;
        }

        public T GetFromName<T>(string name, bool isSymbol = false)
            where T : MaybeNamed
        {
            if (TryGetFromName<T>(name, out var res, isSymbol))
                return res!;

            throw new InvalidOperationException($"No instances of " +
                $"{typeof(T).Name} with {(isSymbol ? "symbol" : "name")} {name}.");
        }



        public void ThrowIfNameNotValid<T>(string name, bool isSymbol = false)
            where T : MaybeNamed
        {
            ThrowIfNameNotValid(name, GetTypeWithinDictionary(typeof(T))!, isSymbol);
        }

        public void ThrowIfNameNotValid(string name, Type type, bool isSymbol = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("MaybeName must not be empty.");

            if (decimal.TryParse(name, out _))
                throw new ArgumentException("MaybeName must not be a number.");

            if (!name.All(char.IsLetterOrDigit))
                throw new ArgumentException("MaybeName must be composed of alphanumeric characters.");

            if (NameAlreadyRegistered(name, type, isSymbol))
            {
                throw new InvalidOperationException($"There is already a {type.Name} " +
                    $"named {name}.");
            }
        }
        public bool NameIsValid<T>(string name, bool isSymbol = false)
            where T : MaybeNamed
        {
            return NameIsValid(name, GetTypeWithinDictionary(typeof(T))!, isSymbol);
        }

        private bool NameIsValid(string name, Type type, bool isSymbol)
        {
            try
            {
                ThrowIfNameNotValid(name, type, isSymbol);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool NameAlreadyRegistered(string name, Type type, bool isSymbol = false)
        {
            type = GetTypeWithinDictionary(type)!;

            IEnumerable<MaybeNamed> matches;
            if (isSymbol)
            {
                matches = from named in MaybeNamedsByType[type]
                          where named.MaybeSymbol == name
                          select named;
            }
            else
            {
                matches = from named in MaybeNamedsByType[type]
                          where named.MaybeName == name
                          select named;
            }

            return matches.Any();
        }

        public BaseQuantity DefineNewBaseQuantity(
            string quantityName,
            string fundamentalUnitName,
            Prefix? unitPrefix = null,
            string? quantitySymbol = null,
            string? unitSymbol = null)
        {
            // TODO
            ThrowIfNameNotValid<Unit>(fundamentalUnitName);
            ThrowIfNameNotValid<Quantity>(quantityName);


            BaseQuantity quantity = new(this, quantityName, quantitySymbol);

            if (unitPrefix is null)
            {
                BaseUnit unit = new(this, fundamentalUnitName, quantity, 1m, unitSymbol);
                quantity.InnerFundamentalUnit = unit;
            }
            else
            {
                var fundamentalMultiplier = 1m / unitPrefix.Multiplier;
                BaseUnit unit = new(this, fundamentalUnitName, quantity, fundamentalMultiplier, unitSymbol);
                quantity.InnerFundamentalUnit = new PrefixedBaseUnit(this, unit, unitPrefix);
            }

            return quantity;
        }

        public IEnumerable<IDerivedUnit> GetAllIDerivedUnitsComposedOf(IBaseUnit baseUnit)
        {
            var allUnits = GetAllMaybeNameds<Unit>().Cast<Unit>();

            IEnumerable<IDerivedUnit> unitsComposedOfGiven =
                from unit in allUnits
                where unit is DerivedUnit &&
                unit.UnitComposition is not null &&
                unit.UnitComposition.Composition.ContainsKey(baseUnit)
                select (DerivedUnit)unit;

            IEnumerable<IDerivedUnit> prefixedUnitsComposedOfGiven =
                from prefixedUnit in PrefixedUnit.PrefixedUnits
                where prefixedUnit is PrefixedDerivedUnit &&
                prefixedUnit.UnitComposition is not null &&
                prefixedUnit.UnitComposition.Composition.ContainsKey(baseUnit)
                select (PrefixedDerivedUnit)prefixedUnit;

            return unitsComposedOfGiven.Union(prefixedUnitsComposedOfGiven);
        }




        public Quantity GetFromBaseComposition(NamedComposition<IUnit> composition)
        {
            var resultingQuantComp = Quantity.Empty.BaseQuantityComposition;
            foreach (var (unit, power) in composition.Composition)
            {
                var quantComp = unit.Quantity.BaseQuantityComposition.Pow(power);
                resultingQuantComp *= quantComp;
            }

            return GetFromBaseComposition(resultingQuantComp);
        }

        public Quantity GetFromBaseComposition(NamedComposition<BaseQuantity> composition)
        {
            if (QuantitiesByComposition.TryGetValue(composition, out var res))
                return res;

            return new DerivedQuantity(this, composition);
        }

        public Unit DefineFromComposition(string name, NamedComposition<IUnit> composition)
        {
            var quantity = GetFromBaseComposition(composition);
            if (quantity is BaseQuantity)
                return new BaseUnit(this, name, composition);
            else
                return new DerivedUnit(this, name, composition);
        }
    }
}
