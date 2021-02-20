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
    }
}
