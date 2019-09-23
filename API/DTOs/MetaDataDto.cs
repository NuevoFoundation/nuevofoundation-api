
using System.Collections.Generic;
using Newtonsoft.Json;

namespace API.DTOs
{
    public class MetaDataDto
    {
        [JsonProperty("impactStats")]
        public List<ImpactStatsDto> ImpactStats { get; set; }
        [JsonProperty("team")]
        public List<TeamDto> Team { get; set; }
    }

    public class ImpactStatsDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("titleBoldened")]
        public string TitleBoldened { get; set; }

        [JsonProperty("stat")]
        public string Stat { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

    }
    public class TeamDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("role")]
        public string Role { get; set; }
        [JsonProperty("quote")]
        public string Quote { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("cartoonImageUrl")]
        public string CartoonImageUrl { get; set; }
        [JsonProperty("realImageUrl")]
        public string RealImageUrl { get; set; }
    }
}
