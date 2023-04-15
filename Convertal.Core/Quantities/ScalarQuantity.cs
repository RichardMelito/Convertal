// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convertal.Core.Extensions;

namespace Convertal.Core;
public abstract class ScalarQuantity : Quantity, IScalar<ScalarQuantity, VectorQuantity>
{
    // Must be implemented by derived types
    public override IScalarUnit FundamentalUnit => throw new NotImplementedException();

    // Must be set at construction by derived types
    protected ScalarComposition<IBaseQuantity> SettableBaseQuantityComposition { get; init; } = null!;
    public override ScalarComposition<IBaseQuantity> BaseQuantityComposition => SettableBaseQuantityComposition;


    public override bool IsVector => false;

    public abstract VectorQuantity? VectorAnalog { get; }

    protected ScalarQuantity(Database database, string? name, string? symbol)
        : base(database, name, symbol)
    {
    }

    public static ScalarQuantity operator *(ScalarQuantity left, ScalarQuantity right)
    {
        var resultingComposition = left.BaseQuantityComposition * right.BaseQuantityComposition;
        return (ScalarQuantity)left.Database.GetFromBaseComposition(resultingComposition);
    }
    public static VectorQuantity operator *(ScalarQuantity scalar, VectorQuantity vector)
    {
        var resultingComposition = scalar.BaseQuantityComposition * vector.BaseQuantityComposition;
        return (VectorQuantity)scalar.Database.GetFromBaseComposition(resultingComposition);
    }
    public static ScalarQuantity operator /(ScalarQuantity left, ScalarQuantity right)
    {
        var resultingComposition = left.BaseQuantityComposition / right.BaseQuantityComposition;
        return (ScalarQuantity)left.Database.GetFromBaseComposition(resultingComposition);
    }

    public ScalarQuantity Pow(decimal power)
    {
        return (ScalarQuantity)Database.GetFromBaseComposition(BaseQuantityComposition.Pow(power));
    }
}

public class ScalarBaseQuantity : ScalarQuantity, IBaseQuantity
{
    internal IScalarBaseUnit? SettableFundamentalUnit { get; set; }
    public override IScalarBaseUnit FundamentalUnit => SettableFundamentalUnit!;

    internal VectorBaseQuantity? SettableVectorAnalog { get; set; }
    public override VectorBaseQuantity? VectorAnalog => SettableVectorAnalog;

    internal ScalarBaseQuantity(Database database, string? name, string? symbol)
        : base(database, name, symbol)
    {
        SettableBaseQuantityComposition = new(this);
        Init();
    }

    // TODO
    public override BaseQuantityProto ToProto()
    {
        return new(Name!, Symbol, FundamentalUnit.Name!);
    }

    public override IOrderedEnumerable<IMaybeNamed> GetAllDependents(ref IEnumerable<IMaybeNamed> toIgnore)
    {
        var res = base.GetAllDependents(ref toIgnore).AsEnumerable();

        var quantsComposedOfThis = from comp_quant in Database.CompositionAndQuantitiesDictionary
                                   where comp_quant.Value is IDerivedQuantity &&
                                   comp_quant.Key.ContainsKey(this)
                                   select comp_quant.Value;

        res = res.Union(quantsComposedOfThis);
        foreach (var dependentQuantity in quantsComposedOfThis.Except(toIgnore))
            res = res.Union(dependentQuantity.GetAllDependents(ref toIgnore));

        res.ThrowIfSetContains(this);
        return res.SortByTypeAndName();
    }
}

public class ScalarDerivedQuantity : ScalarQuantity, IDerivedQuantity
{
    public override IScalarDerivedUnit FundamentalUnit { get; }

    internal VectorDerivedQuantity? SettableVectorAnalog { get; set; }
    public override VectorDerivedQuantity? VectorAnalog => SettableVectorAnalog;

    /// <summary>
    /// To be called only from <see cref="Quantity.GetFromBaseComposition(NamedComposition{BaseQuantity})"/>
    /// </summary>
    internal ScalarDerivedQuantity(
        Database database,
        ScalarComposition<IBaseQuantity> composition,
        string? fundamentalUnitName = null)
        : base(database, null, null)
    {
        SettableBaseQuantityComposition = composition;
        FundamentalUnit = new ScalarDerivedUnit(database, this, fundamentalUnitName);
        Init();
    }

    public override DerivedQuantityProto ToProto()
    {
        return new(
            Name,
            Symbol,
            FundamentalUnit.Name,
            new(BaseQuantityComposition.CompositionAsStringDictionary));
    }
}
