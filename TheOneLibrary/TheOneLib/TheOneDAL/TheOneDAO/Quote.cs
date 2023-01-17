using System;
using System.Text.Json.Serialization;

namespace TheOneLib.TheOneDAL.TheOneDAO
{
    [Serializable]
    public class Quote : TheOneBase
    {
        public String dialog { get; set; }

        [JsonPropertyName("movie")]
        public String movie_id { get; set; }

        [JsonPropertyName("character")]
        public String character_id { get; set; }
    }
}
