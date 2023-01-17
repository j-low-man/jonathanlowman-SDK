using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TheOneLib.TheOneDAL.TheOneDAO
{
    [Serializable]
    public class TheOneContainer<T> where T : TheOneInterface
    {

        public int total { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int page { get; set; }
        public int pages { get; set; }

        [JsonPropertyName("docs")]
        public List<T> items { get; set; }

        public Boolean error { get; set; }

        public String errorMessage { get; set; }

        public TheOneContainer()
        {
            error = false;
            errorMessage = String.Empty;
        }
    }
}