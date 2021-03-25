using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public interface IMaybeNamed : IDisposable
    {
        string? MaybeName { get; }
        IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore);

        internal void DisposeThisAndDependents(bool disposeDependents);
    }
}
