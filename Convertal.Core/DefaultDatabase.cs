// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DecimalMath.DecimalEx;

namespace Convertal.Core;

public class DefaultDatabase
{
    public readonly Database Database;
    public readonly DefaultPrefixes Prefixes;
    public readonly DefaultScalarBaseQuantities ScalarBaseQuantities;
    public readonly DefaultVectorBaseQuantities VectorBaseQuantities;
    public readonly DefaultScalarDerivedQuantities ScalarDerivedQuantities;
    public readonly DefaultVectorDerivedQuantities VectorDerivedQuantities;
    public readonly DefaultScalarBaseUnits ScalarBaseUnits;
    public readonly DefaultVectorBaseUnits VectorBaseUnits;
    public readonly DefaultScalarDerivedUnits ScalarDerivedUnits;
    public readonly DefaultVectorDerivedUnits VectorDerivedUnits;
    public readonly DefaultMeasurementSystems MeasurementSystems;

    public DefaultDatabase(Database database)
    {
        Database = database;
        Prefixes = new(this);
        ScalarBaseQuantities = new(this);
        VectorBaseQuantities = new(database);
        ScalarDerivedQuantities = new(database);
        VectorDerivedQuantities = new(database);
        ScalarBaseUnits = new(database);
        VectorBaseUnits = new(database);
        ScalarDerivedUnits = new(database);
        VectorDerivedUnits = new(database);
        MeasurementSystems = new(database);
    }
}
