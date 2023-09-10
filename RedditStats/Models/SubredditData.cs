using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace RedditStats.Models;

public class SubredditData
{
    [JsonPropertyName("kind")] public string Kind { get; set; } = string.Empty;
    [JsonPropertyName("data")] public JsonObject Data { get; set; } = new();
    [JsonPropertyName("replies")] public JsonObject Replies { get; set; } = new();
}