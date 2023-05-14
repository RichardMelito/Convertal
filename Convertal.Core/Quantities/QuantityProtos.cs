// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Text.Json.Serialization;

namespace Convertal.Core;

public record ScalarBaseQuantityProto(
    string Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string FundamentalUnit,
    [property: JsonPropertyOrder(3)] string? VectorAnalog) : MaybeNamedProto(Name, Symbol);

public record VectorBaseQuantityProto(
    string Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string FundamentalUnit,
    [property: JsonPropertyOrder(3)] string ScalarAnalog) : MaybeNamedProto(Name, Symbol);

public record ScalarDerivedQuantityProto(
    string? Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string? FundamentalUnit,
    [property: JsonPropertyOrder(3)] string? VectorAnalog,
    [property: JsonPropertyOrder(4)] ValueEqualityDictionary<string, decimal> BaseQuantityComposition)
    : MaybeNamedProto(Name, Symbol);

public record VectorDerivedQuantityProto(
    string? Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string? FundamentalUnit,
    [property: JsonPropertyOrder(3)] string? ScalarAnalog,
    [property: JsonPropertyOrder(4)] ValueEqualityDictionary<string, decimal> BaseQuantityComposition)
    : MaybeNamedProto(Name, Symbol);
