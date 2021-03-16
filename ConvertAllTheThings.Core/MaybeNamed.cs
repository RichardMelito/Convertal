using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public enum NameLookupError // TODO
        {
            NoError,
            NoneFound
        }

        public static readonly MaybeNameComparer DefaultComparer = new MaybeNameComparer();

        private static readonly Dictionary<Type, List<MaybeNamed>> s_types_nameds = new();
        private bool _disposedValue;

        public string? MaybeName { get; private set; } = null;
        
        protected MaybeNamed(string? name)
        {
            if (name is null)
                return;

            ThrowIfNameNotValid(name, GetType());

            MaybeName = name;
            s_types_nameds[GetType()].Add(this);
        }


        public void ChangeName(string newName)
        {
            ThrowIfNameNotValid(newName, GetType());

            var needToAddToDictionary = MaybeName is null;
            MaybeName = newName;
            if (needToAddToDictionary)
                s_types_nameds[GetType()].Add(this);
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
            return HashCode.Combine(MaybeName, GetType());
        }


        #region static methods
        public static void ThrowIfNameNotValid<T>(string name)
            where T : MaybeNamed
        {
            ThrowIfNameNotValid(name, typeof(T));
        }

        private static void ThrowIfNameNotValid(string name, Type type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("MaybeName must not be empty.");

            if (decimal.TryParse(name, out _))
                throw new ArgumentException("MaybeName must not be a number.");

            if (!name.All(char.IsLetter))
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

        private static bool NameAlreadyRegistered(string name, Type type)
        {
            var matches = from named in s_types_nameds[type]
                          where named.MaybeName == name 
                          select named;

            return matches.Any();
        }

        public static bool NameIsValid<T>(string name)
            where T : MaybeNamed
        {
            return NameIsValid(name, typeof(T));
        }

        public static bool NameAlreadyRegistered<T>(string name)
            where T : MaybeNamed
        {
            return NameAlreadyRegistered(name, typeof(T));
        }

        public static bool TryGetFromName<T>(
            string name,
            out T? namedObject,
            out NameLookupError error)
            where T : MaybeNamed
        {
            var nameds = s_types_nameds[typeof(T)];
            var matches = (from named in nameds
                           where named.MaybeName == name 
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

        public static T GetFromName<T>(string name)
            where T : MaybeNamed
        {
            NameLookupError error;
            if (TryGetFromName<T>(name, out var res, out error))
                return res!;

            throw error switch
            {
                NameLookupError.NoError => new ApplicationException(),

                NameLookupError.NoneFound => new InvalidOperationException(
                    $"No instances of {typeof(T).Name} with name {name}."),

                _ => new NotImplementedException(),
            };
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
                    s_types_nameds[GetType()].Remove(this);
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
