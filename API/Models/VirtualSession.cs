using System;
using Newtonsoft.Json;

namespace API.Models
{
  public class VirtualSession
  {
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    [JsonProperty(PropertyName = "educatorId")]
    public Guid EducatorId { get; set; }

    [JsonProperty(PropertyName = "volunteerId")]
    public Guid? VolunteerId { get; set; }

    [JsonProperty(PropertyName = "timePreferenceOne")]
    public DateTimeOffset TimePreferenceOne { get; set; }

    [JsonProperty(PropertyName = "timePreferenceTwo")]
    public DateTimeOffset TimePreferenceTwo { get; set; }

    [JsonProperty(PropertyName = "timePreferenceThree")]
    public DateTimeOffset TimePreferenceThree { get; set; }

    [JsonProperty(PropertyName = "timePreferenceSelected")]
    public DateTimeOffset TimePreferenceSelected { get; set; }
  }
}
