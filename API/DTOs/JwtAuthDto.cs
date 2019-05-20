using System;
using Newtonsoft.Json;

namespace API.DTOs
{
  public class JwtAuthDto
  {
    public JwtAuthDto(string jwtToken)
    {
      Token = jwtToken;
    }

    [JsonProperty("token")]
    public string Token { get; set; }
    [JsonProperty("refreshToken")]
    public string RefreshToken { get; set; }
  }
}
