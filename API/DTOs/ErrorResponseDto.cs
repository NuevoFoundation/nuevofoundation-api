using Newtonsoft.Json;

namespace API.DTOs
{
  public class ErrorResponseDto
  {
    public ErrorResponseDto(string message)
    {
      Ok = false;
      Message = message;
    }

    [JsonProperty(PropertyName = "ok")]
    public bool Ok { get; set; }

    [JsonProperty(PropertyName = "message")]
    public object Message { get; set; }
  }
}
