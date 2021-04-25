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

        public string? MaybeSymbol { get; private set; } = null;
        
        protected MaybeNamed(string? name, string? symbol = null)
        {
            if (name is null)
                return;

            ThrowIfNameNotValid(name, GetTypeWithinDictionary());

            MaybeName = name;
            s_types_nameds[GetTypeWithinDictionary()].Add(this);

            if (symbol is not null)
                ChangeSymbol(symbol);
        }

        public abstract IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore);

        public void ChangeName(string newName)
        {
            ThrowIfNameNotValid(newName, GetTypeWithinDictionary());

            var needToAddToDictionary = MaybeName is null;
            MaybeName = newName;
            if (needToAddToDictionary)
                s_types_nameds[GetTypeWithinDictionary()].Add(this);
        }

        public void ChangeSymbol(string symbol)
        {
            if (MaybeName is null)
                throw new InvalidOperationException("Must assign a name before assigning a symbol.");

            ThrowIfNameNotValid(symbol, GetTypeWithinDictionary(), true);
            MaybeSymbol = symbol;
        }

        public void ChangeNameAndSymbol(string newName, string symbol)
        {
            ChangeName(newName);
            ChangeSymbol(symbol);
        }

        public override string ToString()
        {
            return MaybeName ?? base.ToString()!;
        }

        public string ToStringSymbol()
        {
            return MaybeSymbol ?? ToString();
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
        public static IEnumerable<MaybeNamed> GetAllMaybeNameds<T>()
        {
            return s_types_nameds[GetTypeWithinDictionary(typeof(T))];
        }

        // for resetting after unit tests
        internal static void ClearAll()
        {
            var flattenedDictionary = from list in s_types_nameds.Values
                                      from maybeNamed in list
                                      select maybeNamed;

            var sortedFlattenedDictionary = flattenedDictionary.SortByTypeAndName().ToArray();
            foreach (var maybeNamed in sortedFlattenedDictionary)
                maybeNamed.Dispose();
        }

        public static void ThrowIfNameNotValid<T>(string name, bool isSymbol = false)
            where T : MaybeNamed
        {
            ThrowIfNameNotValid(name, GetTypeWithinDictionary(typeof(T)), isSymbol);
        }

        private static void ThrowIfNameNotValid(string name, Type type, bool isSymbol = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("MaybeName must not be empty.");

            if (decimal.TryParse(name, out _))
                throw new ArgumentException("MaybeName must not be a number.");

            if (!name.All(char.IsLetterOrDigit))
                throw new ArgumentException("MaybeName must be composed of alphanumeric characters.");

            if (NameAlreadyRegistered(name, type, isSymbol))
            {
                throw new InvalidOperationException($"There is already a {type.Name} " +
                    $"named {name}.");
            }
        }

        private static bool NameIsValid(string name, Type type, bool isSymbol)
        {
            // TODO
            try
            {
                ThrowIfNameNotValid(name, type, isSymbol);
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

        private static bool NameAlreadyRegistered(string name, Type type, bool isSymbol = false)
        {
            type = GetTypeWithinDictionary(type);

            IEnumerable<MaybeNamed> matches;
            if (isSymbol)
            {
                matches = from named in s_types_nameds[type]
                          where named.MaybeSymbol == name
                          select named;
            }
            else
            {
                matches = from named in s_types_nameds[type]
                          where named.MaybeName == name
                          select named;
            }

            return matches.Any();
        }

        public static bool NameIsValid<T>(string name, bool isSymbol = false)
            where T : MaybeNamed
        {
            return NameIsValid(name, GetTypeWithinDictionary(typeof(T)), isSymbol);
        }

        public static bool NameAlreadyRegistered<T>(string name, bool isSymbol = false)
            where T : MaybeNamed
        {
            return NameAlreadyRegistered(name, GetTypeWithinDictionary(typeof(T)), isSymbol);
        }

        public static bool TryGetFromName<T>(
            string name,
            out T? namedObject,
            bool isSymbol = false)
            where T : MaybeNamed
        {
            var nameds = s_types_nameds[GetTypeWithinDictionary(typeof(T))];
            MaybeNamed[] matches;
            if (isSymbol)
            {
                matches = (from named in nameds
                           where named.MaybeSymbol == name
                           select named).ToArray();
            }
            else
            {
                matches = (from named in nameds
                           where named.MaybeName == name
                           select named).ToArray();
            }

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

        public static T GetFromName<T>(string name, bool isSymbol = false)
            where T : MaybeNamed
        {
            if (TryGetFromName<T>(name, out var res, isSymbol))
                return res!;

            throw new InvalidOperationException($"No instances of " +
                $"{typeof(T).Name} with {(isSymbol ? "symbol" : "name")} {name}.");
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

        
        


        #region IDisposable 
        void IMaybeNamed.DisposeThisAndDependents(bool disposeDependents)
        {
            DisposeBody(disposeDependents);
        }

        protected virtual void DisposeBody(bool disposeDependents)
        {
            if (!_disposedValue)
            {
                // TODO: dispose managed state (managed objects)
                s_types_nameds[GetTypeWithinDictionary()].Remove(this);

                if (disposeDependents)
                {
                    var toIgnore = this.Encapsulate().Cast<IMaybeNamed>();
                    var dependents = GetAllDependents(ref toIgnore).ToArray();
                    foreach (var dependent in dependents)
                        dependent.DisposeThisAndDependents(false);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        protected void Dispose(bool disposing, bool disposeDependents = true)
        {
            if (disposing)
                ((IMaybeNamed)this).DisposeThisAndDependents(disposeDependents);
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
