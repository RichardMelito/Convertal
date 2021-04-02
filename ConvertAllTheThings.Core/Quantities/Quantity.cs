using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public abstract class Quantity : MaybeNamed
    {
        /*  There will never be multiple quantities for something in the same 
         *  way there are multiple units for a quantity. So there's F, C, K, etc. 
         *  BaseUnits for Temperature, but there's only 1 Temperature.
         *  
         *  FundamentalUnit
         *  BaseQuantityComposition
         *  
         *  MaybeName - Unnamed namespace? HasName? Temporary names?
         *      - temporaries have Unnamed namespace and fullname of their composition
         */

        private bool _disposed = false;
        private static readonly Dictionary<NamedComposition<BaseQuantity>, Quantity> s_compositions_quantities = new();
        private bool _initialized = false;

        public bool Disposed => _disposed;

        public static IReadOnlyDictionary<NamedComposition<BaseQuantity>, Quantity> 
            CompositionAndQuantitiesDictionary { get; } = s_compositions_quantities.AsReadOnly();

        public static EmptyQuantity Empty => EmptyQuantity.Empty;

        public abstract IUnit FundamentalUnit { get; }
        public abstract NamedComposition<BaseQuantity> BaseQuantityComposition { get; }

        static Quantity()
        {
            AddTypeToDictionary<Quantity>();
        }
        internal static void InitializeClass() { }

        protected Quantity(
            string? name)
            : base(name)
        {
            
        }

        public override string ToString()
        {
            return MaybeName ?? BaseQuantityComposition.ToString();
        }

        protected void Init()
        {
            if (_initialized)
                throw new ApplicationException($"Quantity {MaybeName ?? "{null}"} is already initialized.");

            if (s_compositions_quantities.ContainsValue(this))
                throw new ApplicationException($"Quantity {MaybeName ?? "{null}"} is already within the dictionary.");

            s_compositions_quantities.Add(BaseQuantityComposition, this);
            _initialized = true;
        }

        public Quantity Pow(decimal power)
        {
            return GetFromBaseComposition(BaseQuantityComposition.Pow(power));
        }


        public static Quantity GetFromBaseComposition(NamedComposition<IUnit> composition)
        {
            var resultingQuantComp = Empty.BaseQuantityComposition;
            foreach (var (unit, power) in composition.Composition)
            {
                var quantComp = unit.Quantity.BaseQuantityComposition.Pow(power);
                resultingQuantComp *= quantComp;
            }

            return GetFromBaseComposition(resultingQuantComp);
        }

        public static Quantity GetFromBaseComposition(NamedComposition<BaseQuantity> composition)
        {
            if (s_compositions_quantities.TryGetValue(composition, out var res))
                return res;

            return new DerivedQuantity(composition);
        }

        public static Quantity MultiplyOrDivide(Quantity lhs, Quantity rhs, bool multiplication)
        {
            var resultingComposition = NamedComposition<BaseQuantity>.MultiplyOrDivide(
                lhs.BaseQuantityComposition,
                rhs.BaseQuantityComposition,
                multiplication: multiplication);

            return GetFromBaseComposition(resultingComposition);
        }

        public static Quantity operator* (Quantity lhs, Quantity rhs)
        {
            return MultiplyOrDivide(lhs, rhs, multiplication: true);
        }

        public static Quantity operator/ (Quantity lhs, Quantity rhs)
        {
            return MultiplyOrDivide(lhs, rhs, multiplication: false);
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            toIgnore = toIgnore.UnionAppend(this);

            var allUnits = GetAllMaybeNameds<Unit>().Cast<Unit>();
            var unitsWithThisQuantity = from unit in allUnits
                                        where unit.Quantity == this
                                        select unit;

            var res = unitsWithThisQuantity.Cast<IMaybeNamed>();
            foreach (var unit in unitsWithThisQuantity.Except(toIgnore))
                res = res.Union(unit.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(this);
            return res.SortByTypeAndName();
        }



        protected override void DisposeBody(bool disposeDependents)
        {
            if (_disposed)
                return;

            if (!s_compositions_quantities.Remove(BaseQuantityComposition))
                throw new ApplicationException(
                    $"Could not remove Quantity {MaybeName ?? "{null}"} with composition " +
                    $"{BaseQuantityComposition} from static dictionary.");

            _disposed = true;
            base.DisposeBody(disposeDependents);
        }
    }
}
