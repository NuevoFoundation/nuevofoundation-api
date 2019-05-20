using System;
using Newtonsoft.Json;

namespace API.Models
{
  public class Member
  {
    [JsonProperty(PropertyName = "id")]
    public Guid Id { get; set; }

    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }

    [JsonProperty(PropertyName = "fullName")]
    public string FullName { get; set; }

    [JsonProperty(PropertyName = "memberType")]
    public string MemberType { get; set; }

    [JsonProperty(PropertyName = "passwordHashed")]
    public string PasswordHashed { get; set; }

    [JsonProperty(PropertyName = "refreshToken")]
    public string RefreshToken { get; set; }

    [JsonProperty(PropertyName = "timezone")]
    public string Timezone { get; set; }

    [JsonProperty(PropertyName = "timezoneOffset")]
    public int TimezoneOffset { get; set; }
  }
}
