using System;
using Newtonsoft.Json;

namespace API.Models
{
    public class MetaData
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("titleBoldened")]
        public string TitleBoldened { get; set; }
        [JsonProperty("stat")]
        public string stat { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("quote")]
        public string Quote { get; set; }
        [JsonProperty("cartoonImageUrl")]
        public string CartoonImageUrl { get; set; }
        [JsonProperty("realImageUrl")]
        public string RealImageUrl { get; set; }
    }
}
