﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertAllTheThings.Core.Extensions;

namespace ConvertAllTheThings.Core
{
    public interface IUnit : IMaybeNamed, IEquatable<IUnit>
    {
        bool IsFundamental => Quantity.FundamentalUnit.Equals(this);
        decimal FundamentalMultiplier { get; }
        Quantity Quantity { get; }
        BaseComposition<IBaseUnit>? MaybeBaseUnitComposition { get; }

        Term ConvertTo(IUnit resultingIUnit)
        {
            if (resultingIUnit.Quantity != Quantity)
                throw new ArgumentException("Can only convert to other units of the same quantity.");

            var magnitude = FundamentalMultiplier / resultingIUnit.FundamentalMultiplier;
            return new Term(magnitude, resultingIUnit);

            /*  converting from km to mm
             *      km multiplier = 1000
             *      mm multiplier = 0.001
             *      1 km = 1,000,000 mm
             *      => multiplier = km.multiplier / mm.multiplier
             */
        }

        Term ConvertToFundamental()
        {
            if (IsFundamental)
                return new Term(1m, this);

            return new Term(FundamentalMultiplier, Quantity.FundamentalUnit);
        }

        static IOrderedEnumerable<IMaybeNamed> GetAllDependents(IUnit unit, ref IEnumerable<IMaybeNamed> toIgnore)
        {
            toIgnore = toIgnore.UnionAppend(unit);

            var dependentQuants =
                from quant in Quantity.CompositionAndQuantitiesDictionary.Values
                where quant.FundamentalUnit == unit
                select quant;

            var res = dependentQuants.Cast<IMaybeNamed>();
            foreach (var quant in dependentQuants.Except(toIgnore))
                res = res.Union(quant.GetAllDependents(ref toIgnore));

            res.ThrowIfSetContains(unit);
            return res.SortByTypeAndName();
        }

        IOrderedEnumerable<IMaybeNamed> IMaybeNamed.GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
        {
            return GetAllDependents(this, ref toIgnore);
        }
    }
}