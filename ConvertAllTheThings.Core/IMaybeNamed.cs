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



        static T? FromString<T>(string str)
            where T : IMaybeNamed
        {
            var type = typeof(T);
            if (type.IsSubclassOf(typeof(MaybeNamed)))
            {
                if (MaybeNamed.TryGetFromName(str, type, out var res))
                    return res!.ForceCast<T>();

                else
                    return default;
            }

            if (type.IsSubclassOf(typeof(PrefixedUnit)))
            {
                var split = str.Split('_');
                if (split.Length != 2)
                    throw new ArgumentException(str);

                var prefix = MaybeNamed.GetFromName<Prefix>(split[0]);
                var unit = MaybeNamed.GetFromName<Unit>(split[1]);
                return PrefixedUnit.GetPrefixedUnit(unit, prefix).ForceCast<T>();
            }

            throw new ArgumentException(str);

            //T? res = default;
            //res = res switch
            //{
            //    MaybeNamed => (T)new Object(),
            //    _ => throw new Exception()
            //};

            //return res;
        }
    }
}
