using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public static class Global
    {
        public static void InitializeAssembly()
        {
            Quantity.InitializeClass();
            Unit.InitializeClass();
            Prefix.InitializeClass();
        }
    }
}
