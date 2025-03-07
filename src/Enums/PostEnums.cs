using System.Text.Json.Serialization;

namespace reichan_api.src.Enums {

    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum Categories
    {
        politic,
        anime,
        games,
        reviews,
        art
    }
}