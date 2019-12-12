using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FlowFitExample.Models.Meeseeks
{
    /// <summary>
    /// Category of a Meeseeks task
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MeeseeksTaskCategory
    {
        Unknown,
        Simple,
        Repeatable,
        LongRunning,
        Jerry
    }
}
