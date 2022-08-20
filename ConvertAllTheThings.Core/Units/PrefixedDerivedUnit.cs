using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConvertAllTheThings.Core
{
    public class PrefixedDerivedUnit : PrefixedUnit, IDerivedUnit
    {
        [JsonConverter(typeof(JsonConverters.ToStringConverter))]
        public new DerivedUnit Unit => (DerivedUnit)base.Unit;

        internal PrefixedDerivedUnit(Database database, DerivedUnit unit, Prefix prefix) 
            : base(database, unit, prefix)
        {
            
        }
    }
}
