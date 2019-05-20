using System;
using Newtonsoft.Json;

namespace API.DTOs
{
  public class CalendarEventDto
  {

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("body")]
    public Body Body { get; set; }

    [JsonProperty("start")]
    public End Start { get; set; }

    [JsonProperty("end")]
    public End End { get; set; }

    [JsonProperty("location")]
    public Location Location { get; set; }

    [JsonProperty("attendees")]
    public Attendee[] Attendees { get; set; }
  }

  public class Attendee
  {
    [JsonProperty("emailAddress")]
    public EmailAddress EmailAddress { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
  }

  public class EmailAddress
  {
    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
  }

  public class Body
  {
    [JsonProperty("contentType")]
    public string ContentType { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
  }

  public class End
  {
    [JsonProperty("dateTime")]
    public DateTimeOffset DateTime { get; set; }

    [JsonProperty("timeZone")]
    public string TimeZone { get; set; }
  }

  public class Location
  {
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }
  }
}
