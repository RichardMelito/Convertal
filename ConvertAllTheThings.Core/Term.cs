using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class Term
    {
        public decimal Magnitude { get; }
        public IUnit Unit { get; }
        public Term(decimal magnitude, IUnit unit)
        {
            Magnitude = magnitude;
            Unit = unit;
        }

        public static Term operator *(decimal multiplier, Term term)
            => new (multiplier * term.Magnitude, term.Unit);

        public static Term operator *(Term term, decimal multiplier)
            => new (multiplier * term.Magnitude, term.Unit);

        public static Term operator /(Term term, decimal divisor)
            => new (term.Magnitude / divisor, term.Unit);

        public static Term operator *(Term lhs, Term rhs)
        {

        }
    }
}
