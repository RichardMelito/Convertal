// Created by Richard Melito and licensed to you under The Clear BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DecimalMath.DecimalEx;

namespace Convertal.Core.Defaults;

public class DefaultDatabaseWrapper
{
    public readonly Database Database;
    public readonly DefaultPrefixes Prefixes;
    public readonly ScalarBaseQuantities ScalarBaseQuantities;
    public readonly VectorBaseQuantities VectorBaseQuantities;
    public readonly ScalarDerivedQuantities ScalarDerivedQuantities;
    public readonly VectorDerivedQuantities VectorDerivedQuantities;
    public readonly ScalarBaseUnits ScalarBaseUnits;
    public readonly VectorBaseUnits VectorBaseUnits;
    public readonly ScalarDerivedUnits ScalarDerivedUnits;
    public readonly VectorDerivedUnits VectorDerivedUnits;
    public readonly MeasurementSystems MeasurementSystems;

    public DefaultDatabaseWrapper(Database database)
    {
        Database = database;
        Prefixes = new(this);
        ScalarBaseQuantities = new(this);
        VectorBaseQuantities = new(this);
        ScalarDerivedQuantities = new(this);
        VectorDerivedQuantities = new(database);
        ScalarBaseUnits = new(database);
        VectorBaseUnits = new(database);
        ScalarDerivedUnits = new(database);
        VectorDerivedUnits = new(database);
        MeasurementSystems = new(database);
    }
}
