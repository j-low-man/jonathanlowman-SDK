using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TheOneLib
{
    [Serializable]
    public class MovieContainer
    {

        public int total { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int page { get; set; }
        public int pages { get; set; }

        [JsonPropertyName("docs")]
        public List<Movie> movies { get; set; }

    }
}