// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Text.Json.Serialization;

namespace Convertal.Core;

public record DerivedQuantityProto(
    string? Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string? FundamentalUnit,
    [property: JsonPropertyOrder(3)] ValueEqualityDictionary<string, decimal> BaseQuantityComposition)
    : MaybeNamedProto(Name, Symbol);
