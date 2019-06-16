using System;
using Newtonsoft.Json;

namespace API.DTOs
{
  public class GraphAuthDto
  {
    [JsonProperty(PropertyName = "token_type")]
    public string TokenType { get; set; }

    [JsonProperty(PropertyName = "expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty(PropertyName = "ext_expires_in")]
    public string ExtExpiresIn { get; set; }

    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; set; }
  }
}
