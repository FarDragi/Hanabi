using System.Text.Json.Serialization;

namespace FarDragi.Hanabi.Models;

public record UserJoinDto(
    [property:JsonPropertyName("user_id")] string UserId, 
    [property:JsonPropertyName("source_invite_code")] string SourceInviteCode);