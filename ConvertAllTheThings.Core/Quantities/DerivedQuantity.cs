using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public record DerivedQuantityProto(
        string? Name, 
        string? Symbol, 
        string FundamentalUnit, 
        Dictionary<string, decimal> BaseQuantityComposition) 
        : MaybeNamedProto(Name, Symbol);

    public class DerivedQuantity : Quantity, IDerived
    {
        [JsonPropertyOrder(2)]
        [JsonConverter(typeof(JsonConverters.ToStringConverter))]
        public override IUnit FundamentalUnit { get; }

        [JsonPropertyOrder(3)]
        public override NamedComposition<BaseQuantity> BaseQuantityComposition { get; }

        /// <summary>
        /// To be called only from <see cref="Quantity.GetFromBaseComposition(NamedComposition{BaseQuantity})"/>
        /// </summary>
        /// <param name="composition"></param>
        internal DerivedQuantity(Database database, NamedComposition<BaseQuantity> composition)
            : base(database, null, null)
        {
            BaseQuantityComposition = composition;
            FundamentalUnit = new DerivedUnit(database, this);
            Init();
        }

        public DerivedQuantityProto Proto => ToProto();

        public override DerivedQuantityProto ToProto()
        {
            return new(
                Name, 
                Symbol, 
                FundamentalUnit.ToString()!, 
                BaseQuantityComposition.CompositionAsStringDictionary);
        }
    }
}
