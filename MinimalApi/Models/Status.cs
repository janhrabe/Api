using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MinimalApi.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Status
    {
        [DefaultValue(true)]
        New,
        Active,
        Done
    }
}