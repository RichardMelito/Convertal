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
        private static readonly Dictionary<BaseComposition<BaseQuantity>, Quantity> s_compositions_quantities = new();
        private bool _initialized = false;

        public static IReadOnlyDictionary<BaseComposition<BaseQuantity>, Quantity> 
            CompositionAndQuantitiesDictionary { get; } = s_compositions_quantities.AsReadOnly();

        public static EmptyQuantity Empty => EmptyQuantity.Empty;

        public abstract IUnit FundamentalUnit { get; }
        public abstract BaseComposition<BaseQuantity> BaseQuantityComposition { get; }

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


        public static Quantity GetFromBaseComposition(BaseComposition<IBaseUnit> composition)
        {
            var quantityComposition = BaseComposition<BaseQuantity>.
                CreateFromExistingBaseComposition(
                composition,
                baseUnit => baseUnit.BaseQuantity);

            return GetFromBaseComposition(quantityComposition);
        }

        public static Quantity GetFromBaseComposition(BaseComposition<BaseQuantity> composition)
        {
            if (s_compositions_quantities.TryGetValue(composition, out var res))
                return res;

            return new DerivedQuantity(composition);
        }

        public static Quantity MultiplyOrDivide(Quantity lhs, Quantity rhs, bool multiplication)
        {
            var resultingComposition = BaseComposition<BaseQuantity>.MultiplyOrDivide(
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



        protected override void Dispose(bool disposing)
        {
            // TODO: dispose of associated units and dependent derived quantities. Will be messy


            if (_disposed)
                return;

            if (!s_compositions_quantities.Remove(BaseQuantityComposition))
                throw new ApplicationException(
                    $"Could not remove Quantity {MaybeName ?? "{null}"} with composition " +
                    $"{BaseQuantityComposition} from static dictionary.");

            if (this is BaseQuantity thisAsBase)
            {
                var quantsComposedOfThis = from comp_quant in s_compositions_quantities
                                           where comp_quant.Value is DerivedQuantity &&
                                           comp_quant.Key.Composition.Keys.Contains(thisAsBase)
                                           select (DerivedQuantity)comp_quant.Value;

                foreach (var quantToDispose in quantsComposedOfThis.ToArray())
                    quantToDispose.Dispose();
            }

            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
