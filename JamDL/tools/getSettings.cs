// Love you quicktype
// https://app.quicktype.io/
namespace getSettings
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class UserSettings
    {
        [JsonProperty("ffmpeg32", NullValueHandling = NullValueHandling.Ignore)]
        public string Ffmpeg32 { get; set; }

        [JsonProperty("ffmpeg32Hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Ffmpeg32Hash { get; set; }

        [JsonProperty("ffmpeg64", NullValueHandling = NullValueHandling.Ignore)]
        public string Ffmpeg64 { get; set; }

        [JsonProperty("ffmpeg64Hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Ffmpeg64Hash { get; set; }

        [JsonProperty("ffmpegVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string FfmpegVersion { get; set; }
    }

    public partial class UserSettings
    {
        public static UserSettings FromJson(string json) => JsonConvert.DeserializeObject<UserSettings>(json, getSettings.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this UserSettings self) => JsonConvert.SerializeObject(self, getSettings.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
