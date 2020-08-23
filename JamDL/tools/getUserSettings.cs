// Love you quicktype
// https://app.quicktype.io/

namespace getUserSettings
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using J = Newtonsoft.Json.JsonPropertyAttribute;

    public partial class UserSettings
    {
        [J("info")]
        public Info[] Info { get; set; }
    }

    public partial class Info
    {
        [J("jamdlAutoU")]
        public string JamdlAutoU { get; set; }

        [J("depAutoU")]
        public string DepAutoU { get; set; }

        [J("faces")]
        public string Faces { get; set; }

        [J("finnishedFolderDialog")]
        public string FinnishedFolderDialog { get; set; }

        [J("DefultDlPath")]
        public string DefultDlPath { get; set; }
    }

    public partial class UserSettings
    {
        public static UserSettings FromJson(string json) => JsonConvert.DeserializeObject<UserSettings>(json, getUserSettings.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this UserSettings self) => JsonConvert.SerializeObject(self, getUserSettings.Converter.Settings);
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
