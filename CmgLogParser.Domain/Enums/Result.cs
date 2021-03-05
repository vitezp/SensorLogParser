using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CmgLogParser.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Result
    {
        [EnumMember(Value = "precise")] Precise,
        [EnumMember(Value = "very precise")] VeryPrecise,
        [EnumMember(Value = "ultra precise")] UltraPrecise,
        [EnumMember(Value = "nodata")] NoData,
        [EnumMember(Value = "keep")] Keep,
        [EnumMember(Value = "discard")] Discard,
    }
}