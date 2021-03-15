using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class Prefix : MaybeNamed, INamed
    {
        static Prefix()
        {
            AddTypeToDictionary<Prefix>();
        }

        public decimal Multiplier { get; }

        public Prefix(decimal multiplier, string name)
            : base (name)
        {
            Multiplier = multiplier;
        }
    }
}
