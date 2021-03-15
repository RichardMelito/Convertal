using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class BaseUnit : Unit, IBaseUnit
    {
        // TODO check that BaseUnits can be retrieved from Named<Unit>.GetFromName

        internal BaseUnit(string name, BaseQuantity quantity, decimal fundamentalMultiplier)
            : base(name, quantity, fundamentalMultiplier)
        {
            //Quantity = quantity;
        }
    }
}
