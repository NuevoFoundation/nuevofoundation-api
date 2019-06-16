using System;
using Newtonsoft.Json;

namespace API.DTOs
{
  public class LoginDto
  {
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; }

  }
}
