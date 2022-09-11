using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public record EmptyQuantityProto() : MaybeNamedProto(null, null);

    public sealed class EmptyQuantity : Quantity, IEquatable<EmptyQuantity>
    {
        public override EmptyUnit FundamentalUnit => Database.EmptyUnit;

        public override NamedComposition<BaseQuantity> BaseQuantityComposition => NamedComposition<BaseQuantity>.Empty;

        internal EmptyQuantity(Database database)
            : base(database, null, null)
        {
            Init();
        }

        public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            toIgnore = toIgnore.UnionAppend(this);
            return Array.Empty<IMaybeNamed>().SortByTypeAndName();
        }

        protected override void DisposeBody(bool disposeDependents)
        {
            // The Database.EmptyQuantity cannot be disposed
            return;
        }

        public override EmptyQuantityProto ToProto()
        {
            return new();
        }

        public bool Equals(EmptyQuantity? other) => other is not null;
    }
}