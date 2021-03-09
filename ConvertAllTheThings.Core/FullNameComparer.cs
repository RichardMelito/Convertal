using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public class FullNameComparer : Comparer<INamed>
    {
        public static readonly FullNameComparer DefaultComparer = new();

        public override int Compare(INamed? x, INamed? y)
        {
            if (x is null || y is null)
            {
                if ((x is null) && (y is null))
                    return 0;

                if (x is null)
                    return -1;

                return 1;
            }

            return string.Compare(x.FullName, y.FullName);
        }
    }
}
