using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FlowFitExample.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MeeseeksTaskType
    {
        Unknown,
        Simple,
        Repeatable,
        LongRunning,
        Jerry
    }
}
