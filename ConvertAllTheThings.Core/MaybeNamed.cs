using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public abstract class MaybeNamed : IMaybeNamed, IDisposable, IComparable<MaybeNamed>, IEquatable<MaybeNamed>
    {
        public class MaybeNameComparer : Comparer<IMaybeNamed>
        {
            public override int Compare(IMaybeNamed? x, IMaybeNamed? y)
            {
                return PerformCompare(x, y);
            }

            public static int PerformCompare(IMaybeNamed? x, IMaybeNamed? y)
            {
                if (x is null || y is null)
                {
                    if ((x is null) && (y is null))
                        return 0;

                    if (x is null)
                        return -1;

                    return 1;
                }

                return string.Compare(x.MaybeName, y.MaybeName);
            }
        }

        public static readonly MaybeNameComparer DefaultComparer = new();

        private static readonly Dictionary<Type, List<MaybeNamed>> s_types_nameds = new();
        private bool _disposedValue;

        public string? MaybeName { get; private set; } = null;
        
        protected MaybeNamed(string? name)
        {
            if (name is null)
                return;

            ThrowIfNameNotValid(name, GetTypeWithinDictionary());

            MaybeName = name;
            s_types_nameds[GetTypeWithinDictionary()].Add(this);
        }

        public abstract IEnumerable<IMaybeNamed> GetAllDependents();

        public void ChangeName(string newName)
        {
            ThrowIfNameNotValid(newName, GetTypeWithinDictionary());

            var needToAddToDictionary = MaybeName is null;
            MaybeName = newName;
            if (needToAddToDictionary)
                s_types_nameds[GetTypeWithinDictionary()].Add(this);
        }

        public override string ToString()
        {
            return MaybeName ?? base.ToString()!;
        }

        public int CompareTo(MaybeNamed? other)
        {
            return DefaultComparer.Compare(this, other);
        }

        public bool Equals(MaybeNamed? other)
        {
            return ReferenceEquals(this, other);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MaybeName, GetTypeWithinDictionary());
        }


        #region static methods
        public static IEnumerable<T> GetAllMaybeNameds<T>()
        {
            return s_types_nameds[GetTypeWithinDictionary(typeof(T))].Cast<T>();
        }

        // for resetting after unit tests
        internal static void ClearAll()
        {
            var flattenedDictionary = from list in s_types_nameds.Values
                                      from maybeNamed in list
                                      select maybeNamed;

            foreach (var maybeNamed in flattenedDictionary.ToArray())
                maybeNamed.Dispose();
        }

        public static void ThrowIfNameNotValid<T>(string name)
            where T : MaybeNamed
        {
            ThrowIfNameNotValid(name, GetTypeWithinDictionary(typeof(T)));
        }

        private static void ThrowIfNameNotValid(string name, Type type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("MaybeName must not be empty.");

            if (decimal.TryParse(name, out _))
                throw new ArgumentException("MaybeName must not be a number.");

            if (!name.All(char.IsLetterOrDigit))
                throw new ArgumentException("MaybeName must be composed of alphanumeric characters.");

            if (NameAlreadyRegistered(name, type))
            {
                throw new InvalidOperationException($"There is already a {type.Name} " +
                    $"named {name}.");
            }
        }

        private static bool NameIsValid(string name, Type type)
        {
            // TODO
            try
            {
                ThrowIfNameNotValid(name, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Type GetTypeWithinDictionary(Type type)
        {
            var originalType = type;

            while (!s_types_nameds.ContainsKey(type))
            {
                if (type.BaseType is null)
                    throw new ArgumentException($"Neither type {originalType.Name} " +
                        $"nor any of its base types are within the name lookup dictionary.");

                type = type.BaseType;
            }

            return type;
        }

        public Type GetTypeWithinDictionary()
        {
            return GetTypeWithinDictionary(GetType());
        }

        private static bool NameAlreadyRegistered(string name, Type type)
        {
            type = GetTypeWithinDictionary(type);

            var matches = from named in s_types_nameds[type]
                          where named.MaybeName == name 
                          select named;

            return matches.Any();
        }

        public static bool NameIsValid<T>(string name)
            where T : MaybeNamed
        {
            return NameIsValid(name, GetTypeWithinDictionary(typeof(T)));
        }

        public static bool NameAlreadyRegistered<T>(string name)
            where T : MaybeNamed
        {
            return NameAlreadyRegistered(name, GetTypeWithinDictionary(typeof(T)));
        }

        public static bool TryGetFromName<T>(
            string name,
            out T? namedObject)
            where T : MaybeNamed
        {
            var nameds = s_types_nameds[GetTypeWithinDictionary(typeof(T))];
            var matches = (from named in nameds
                           where named.MaybeName == name 
                           select named).ToArray();

            if (matches.Length == 1)
            {
                namedObject = (T)matches.First();
                return true;
            }
            else if (matches.Length == 0)
            {
                namedObject = null;
                return false;
            }
            else
            {
                throw new ApplicationException();
            }
        }

        public static T GetFromName<T>(string name)
            where T : MaybeNamed
        {
            if (TryGetFromName<T>(name, out var res))
                return res!;

            throw new InvalidOperationException($"No instances of " +
                $"{typeof(T).Name} with name {name}.");
        }

        protected static void AddTypeToDictionary<T>()
            where T : MaybeNamed
        {
            s_types_nameds.Add(typeof(T), new List<MaybeNamed>());
        }

        public static bool operator ==(MaybeNamed? lhs, MaybeNamed? rhs)
        {
            return ReferenceEquals(lhs, rhs);
        }

        public static bool operator !=(MaybeNamed? lhs, MaybeNamed? rhs)
        {
            return !(lhs == rhs);
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
                    s_types_nameds[GetTypeWithinDictionary()].Remove(this);

                    var dependents = GetAllDependents().ToArray();
                    foreach (var dependent in dependents)
                        dependent.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MaybeNamed()
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
