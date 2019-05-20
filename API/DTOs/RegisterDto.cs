using System;
using Newtonsoft.Json;

namespace API.DTOs
{
  public class RegisterDto
  {
    [JsonProperty("fullName")]
    public string FullName { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }
    [JsonProperty("memberType")]
    public string MemberType { get; set; }
  }
}
