using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public interface IMaybeNamed
    {
        string? MaybeName { get; }
    }
}
