using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public interface INamed
    {
        public enum NameLookupError
        {
            NoError,
            NoneFound,
            NeedNamespace
        }

        private static readonly Dictionary<Type, List<INamed>> s_types_nameds = new();

        public string Name { get; }
        public string NameSpace { get; }

        public static bool TryGetFromName<T>(
            string name, 
            string nameSpace, 
            out T? namedObject,
            out NameLookupError error) 
            where T : class, INamed
        {
            var nameds = s_types_nameds[typeof(T)];
            var matches = (from named in nameds
                          where named.Name == name && named.NameSpace == nameSpace
                          select named).ToArray();

            if (matches.Length == 1)
            {
                namedObject = matches.First() as T;
                error = NameLookupError.NoError;
                return true;
            }
            else if (matches.Length == 0)
            {
                namedObject = null;
                error = NameLookupError.NoneFound;
                return false;
            }
            else
            {
                throw new ApplicationException();
            }
        }

        public static bool TryGetFromName<T>(
            string name,
            out T? namedObject,
            out NameLookupError error)
            where T : class, INamed
        {
            var nameds = s_types_nameds[typeof(T)];
            var matches = (from named in nameds
                           where named.Name == name
                           select named).ToArray();

            if (matches.Length == 1)
            {
                namedObject = matches.First() as T;
                error = NameLookupError.NoError;
                return true;
            }
            else if (matches.Length == 0)
            {
                namedObject = null;
                error = NameLookupError.NoneFound;
                return false;
            }
            else
            {
                namedObject = null;
                error = NameLookupError.NeedNamespace;
                return false;
            }
        }

        public static T GetFromName<T>(string name, string? nameSpace = null)
            where T : class, INamed
        {
            NameLookupError error;
            if (nameSpace is null)
            {
                if (TryGetFromName<T>(name, out var res, out error))
                    return res!;
            }
            else
            {
                if (TryGetFromName<T>(name, nameSpace, out var res, out error))
                    return res!;
            }

            switch (error)
            {
                case NameLookupError.NoError:
                    throw new ApplicationException();

                case NameLookupError.NoneFound:
                    throw new InvalidOperationException($"No instances of " +
                        $"{typeof(T).Name} with name {name}.");

                case NameLookupError.NeedNamespace:
                    throw new InvalidOperationException($"Too many instances of " +
                        $"{typeof(T).Name} with name {name}. " +
                        $"Must give a NameSpace to specify.");

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
