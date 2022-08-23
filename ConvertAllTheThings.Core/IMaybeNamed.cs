using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public interface IMaybeNamed : IDisposable
    {
        string? MaybeName { get; }
        string? MaybeSymbol { get; }

        string ToStringSymbol(); 

        IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore);

        internal void DisposeThisAndDependents(bool disposeDependents);

        public T CastAndChangeNameAndSymbol<T>(string name, string? symbol)
            where T : MaybeNamed
        {
            var res = (T)this;
            res.ChangeNameAndSymbol(name, symbol);
            return res;
        }
    }
}
