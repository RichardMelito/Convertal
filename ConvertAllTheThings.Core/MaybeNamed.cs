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
    public abstract class MaybeNamed : IMaybeNamed
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

                return string.Compare(x.Name, y.Name);
            }
        }

        public static readonly MaybeNameComparer DefaultComparer = new();

        private bool _disposedValue;

        [JsonIgnore]
        internal Database Database { get; }

        public string? Name { get; private set; } = null;

        public string? Symbol { get; private set; } = null;
        
        protected MaybeNamed(Database database, string? name, string? symbol = null)
        {
            Database = database;
            Database.AddTypeToDictionary(GetDatabaseType());
            //Database.AddToSerializationList(this);

            if (name is null)
                return;

            Database.ThrowIfNameNotValid(name, GetTypeWithinDictionary());

            Name = name;
            Database.MaybeNamedsByType[GetTypeWithinDictionary()].Add(this);

            if (symbol is not null)
                ChangeSymbol(symbol);
        }

        protected virtual Type GetDatabaseType() => GetType();

        public abstract IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore);

        public void ChangeName(string newName)
        {
            Database.ThrowIfNameNotValid(newName, GetTypeWithinDictionary());

            var needToAddToDictionary = Name is null;
            Name = newName;
            if (needToAddToDictionary)
                Database.MaybeNamedsByType[GetTypeWithinDictionary()].Add(this);
        }

        public void ChangeSymbol(string symbol)
        {
            if (Name is null)
                throw new InvalidOperationException("Must assign a name before assigning a symbol.");

            Database.ThrowIfNameNotValid(symbol, GetTypeWithinDictionary(), true);
            Symbol = symbol;
        }

        public void ChangeNameAndSymbol(string newName, string? symbol = null)
        {
            ChangeName(newName);
            if (symbol is not null)
                ChangeSymbol(symbol);
        }

        public override string ToString()
        {
            return Name ?? base.ToString()!;
        }

        public string ToStringSymbol()
        {
            return Symbol ?? ToString();
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
            if (lhs is null && rhs is null)
                return true;
            else if (lhs is not null && rhs is not null)
                return lhs.Equals(rhs);
            else
                return false;
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
                //Database.RemoveFromSerializationList(this);

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



        public override bool Equals(object? obj)
        {
            return ((IMaybeNamed)this).Equals(obj as IMaybeNamed);
        }

        public override int GetHashCode()
        {
            return ((IMaybeNamed)this).CalculateHashCode();
        }
    }
}
