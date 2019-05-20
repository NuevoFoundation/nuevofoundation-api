using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace API.Http
{
  public class Http
  {
    public Http()
    {
    }

    private static readonly HttpClient _httpClient = new HttpClient();

    private async Task<T> MakeRequestAsync<T>(HttpRequestMessage request)
    {
      var result = await _httpClient.SendAsync(request);
      return await ParseResponse<T>(result);
    }

    public async Task<T> Post<T>(string requestUri, string body, Dictionary<string, string> headers = null, bool formMediaType = false)
    {
      var httpRequestMessage = BuildRequestMessage(requestUri, body, HttpMethod.Post, headers, formMediaType);

      return await MakeRequestAsync<T>(httpRequestMessage);
    }

    private HttpRequestMessage BuildRequestMessage(string requestUri, string body, HttpMethod method, Dictionary<string, string> headers, bool formMediaType)
    {
      var mediaType = formMediaType ? "application/x-www-form-urlencoded" : "application/json";
      var httpRequestMessage = new HttpRequestMessage();
      httpRequestMessage.Method = method;
      httpRequestMessage.RequestUri = new System.Uri(requestUri);
      httpRequestMessage.Content = new StringContent(
        body,
        Encoding.UTF8,
        mediaType
      );

      if(headers != null)
      {
        foreach (var header in headers)
        {
          httpRequestMessage.Headers.Add(header.Key, header.Value);
        }
      }

      return httpRequestMessage;
    }

    private async Task<T> ParseResponse<T>(HttpResponseMessage result)
    {
      var responseJson = await result.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(responseJson);
    }
  }
}
