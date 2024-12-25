using System.Text.Json.Serialization;

namespace CleanChat;

public class Config {
    [JsonInclude] public bool Enabled = true;
}
