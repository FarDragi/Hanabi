using System.Text.Json.Serialization;

namespace FarDragi.Hanabi.Models;

public record UserIdsDto([property:JsonPropertyName("user_ids")] IEnumerable<string> UserIds);