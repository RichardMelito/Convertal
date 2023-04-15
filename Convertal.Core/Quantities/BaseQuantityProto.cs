// Created by Richard Melito and licensed to you under The Clear BSD License.

using System.Text.Json.Serialization;

namespace Convertal.Core;

public record BaseQuantityProto(
    string Name,
    string? Symbol,
    [property: JsonPropertyOrder(2)] string FundamentalUnit) : MaybeNamedProto(Name, Symbol);
