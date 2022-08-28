using System.Collections.Generic;

namespace ConvertAllTheThings.Core;

// Just exists to help with serialization
public interface INamedComposition
{
    public IReadOnlyDictionary<string, decimal> Composition { get; }
}
