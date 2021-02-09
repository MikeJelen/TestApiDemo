using System.Text.Json.Serialization;

namespace TestApiDemo.Enumerations
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CreatedFilter
    {
        Newest = 1,
        Oldest = 2
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuantityFilter
    {
        Highest = 1,
        Lowest = 2
    }
}
