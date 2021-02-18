using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAllTheThings.Core
{
    public abstract class Named : IDisposable
    {
        public enum NameLookupError
        {
            NoError,
            NoneFound,
            NeedNamespace
        }

        private static readonly Dictionary<Type, List<Named>> s_types_nameds = new();
        private bool _disposedValue;

        public string Name { get; private set; }
        public string NameSpace { get; private set; }
        public string FullName => NameSpace + "." + Name;

        protected Named(string name, string nameSpace)
        {
            ThrowIfNameAndNameSpaceNotValid(name, nameSpace, GetType());

            Name = name;
            NameSpace = nameSpace;
            s_types_nameds[GetType()].Add(this);
        }


        public void ChangeNameAndNameSpace(string newName, string newNameSpace)
        {
            ThrowIfNameAndNameSpaceNotValid(newName, newNameSpace, GetType());

            Name = newName;
            NameSpace = newNameSpace;
        }



        #region static methods
        private static void ThrowIfNameAndNameSpaceNotValid(string name, string nameSpace, Type type)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(nameSpace))
                throw new ArgumentException("Name and NameSpace must not be empty.");

            if (NameAlreadyRegistered(name, nameSpace, type))
            {
                throw new InvalidOperationException($"There is already a {type.Name} " +
                    $"named {name} in the {nameSpace} namespace.");
            }
        }

        private static bool NameAndNameSpaceValid(string name, string nameSpace, Type type)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(nameSpace))
                return false;

            if (NameAlreadyRegistered(name, nameSpace, type))
                return false;

            return true;
        }

        private static bool NameAlreadyRegistered(string name, string nameSpace, Type type)
        {
            var matches = from named in s_types_nameds[type]
                          where named.Name == name && named.NameSpace == nameSpace
                          select named;

            return matches.Any();
        }

        public static bool NameAndNameSpaceValid<T>(string name, string nameSpace)
            where T : Named
        {
            return NameAndNameSpaceValid(name, nameSpace, typeof(T));
        }

        public static bool NameAlreadyRegistered<T>(string name, string nameSpace)
            where T : Named
        {
            return NameAlreadyRegistered(name, nameSpace, typeof(T));
        }

        public static bool TryGetFromName<T>(
            string name,
            string nameSpace,
            out T? namedObject,
            out NameLookupError error)
            where T : Named
        {
            var nameds = s_types_nameds[typeof(T)];
            var matches = (from named in nameds
                           where named.Name == name && named.NameSpace == nameSpace
                           select named).ToArray();

            if (matches.Length == 1)
            {
                namedObject = (T)matches.First();
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
            where T : Named
        {
            var nameds = s_types_nameds[typeof(T)];
            var matches = (from named in nameds
                           where named.Name == name
                           select named).ToArray();

            if (matches.Length == 1)
            {
                namedObject = (T)matches.First();
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
            where T : Named
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

            throw error switch
            {
                NameLookupError.NoError => new ApplicationException(),

                NameLookupError.NoneFound => new InvalidOperationException(
                    $"No instances of {typeof(T).Name} with name {name}."),

                NameLookupError.NeedNamespace => new InvalidOperationException(
                    $"Too many instances of {typeof(T).Name} with name {name}. " +
                    $"Must give a NameSpace to specify."),

                _ => new NotImplementedException(),
            };
        }

        protected static void AddTypeToDictionary<T>()
            where T : Named
        {
            if (!s_types_nameds.ContainsKey(typeof(T)))
                s_types_nameds[typeof(T)] = new List<Named>();
            else
                throw new InvalidOperationException();
        }
        #endregion

        
        


        #region IDisposable boilerplate
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    s_types_nameds[GetType()].Remove(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Named()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
