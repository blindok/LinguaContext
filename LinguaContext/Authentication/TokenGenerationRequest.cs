using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace LinguaContext.Authentication;

public class TokenGenerationRequest
{
    [JsonPropertyName("userid")]
    public int UserId { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("role")]
    public string Role { get; set; }
    //[JsonPropertyName("customClaims")]
    //public Dictionary<string, object> CustomClaims { get; set; } = new Dictionary<string, object>();
}