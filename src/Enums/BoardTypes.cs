using System.Text.Json.Serialization;

namespace reichan_api.src.Enums {
    
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum BoardTypes
    {
        politic,
        anime,
        games,
        reviews,
        art
    }
}