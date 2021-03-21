using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core.Extensions
{
    public static class Extensions
    {
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dict)
            where TKey : notnull
        {
            return new ReadOnlyDictionary<TKey, TValue>(dict);
        }

        public static IOrderedEnumerable<T> SortByTypeAndName<T>(this IEnumerable<T> toSort)
            where T: notnull, IMaybeNamed
        {
            var res = toSort.OrderByDescending((x) =>
            {
                return x switch
                {
                    BaseQuantity => 300,
                    DerivedQuantity => 200,
                    IBaseUnit { IsFundamental: true } => 100,
                    IDerivedUnit { IsFundamental: true } => 80,
                    IBaseUnit => 50,
                    IDerivedUnit => 30,
                    _ => 0
                };
            });
            res = res.ThenBy(x=>x, MaybeNamed.DefaultComparer);

            return res;
        }

        public static IEnumerable<T> UnionAppend<T>(this IEnumerable<T> set, T toAppend)
        {
            if (set.Contains(toAppend))
                return set;

            return set.Append(toAppend);
        }

        public static IEnumerable<T> Encapsulate<T>(this T toEncapsulate)
        {
            return new T[] { toEncapsulate };
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> set, T toExcept)
        {
            return set.Except(toExcept.Encapsulate());
        }

        public static void ThrowIfSetContains<T>(this IEnumerable<T> set, T toCheck)
        {
            if (set.Contains(toCheck))
                throw new ApplicationException();
        }
    }
}
