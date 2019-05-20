using System;
using Newtonsoft.Json;

namespace API.DTOs
{
  public class JwtUserClaimsDto
  {
    public JwtUserClaimsDto(string id)
    {
      Id = id;
    }

    [JsonProperty("id")]
    public string Id { get; set; }
  }
}
