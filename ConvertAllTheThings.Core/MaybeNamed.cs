using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;
using System.Text.Json.Serialization;

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

        private bool _disposedValue;

        [JsonIgnore]
        internal Database Database { get; }

        public string? MaybeName { get; private set; } = null;

        public string? MaybeSymbol { get; private set; } = null;
        
        protected MaybeNamed(Database database, string? name, string? symbol = null)
        {
            Database = database;
            if (name is null)
                return;

            database.AddTypeToDictionary(GetDatabaseType());

            Database.ThrowIfNameNotValid(name, GetTypeWithinDictionary());

            MaybeName = name;
            Database.MaybeNamedsByType[GetTypeWithinDictionary()].Add(this);

            if (symbol is not null)
                ChangeSymbol(symbol);
        }

        protected virtual Type GetDatabaseType() => GetType();

        public abstract IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore);

        public void ChangeName(string newName)
        {
            Database.ThrowIfNameNotValid(newName, GetTypeWithinDictionary());

            var needToAddToDictionary = MaybeName is null;
            MaybeName = newName;
            if (needToAddToDictionary)
                Database.MaybeNamedsByType[GetTypeWithinDictionary()].Add(this);
        }

        public void ChangeSymbol(string symbol)
        {
            if (MaybeName is null)
                throw new InvalidOperationException("Must assign a name before assigning a symbol.");

            Database.ThrowIfNameNotValid(symbol, GetTypeWithinDictionary(), true);
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
        

        //// for resetting after unit tests
        //internal static void ClearAll()
        //{
        //    var flattenedDictionary = from list in Database.MaybeNamedsByType.Values
        //                              from maybeNamed in list
        //                              select maybeNamed;

        //    var sortedFlattenedDictionary = flattenedDictionary.SortByTypeAndName().ToArray();
        //    foreach (var maybeNamed in sortedFlattenedDictionary)
        //        maybeNamed.Dispose();
        //}

        public Type GetTypeWithinDictionary()
        {
            return Database.GetTypeWithinDictionary(GetType())!;
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
                Database.MaybeNamedsByType[GetTypeWithinDictionary()].Remove(this);

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
