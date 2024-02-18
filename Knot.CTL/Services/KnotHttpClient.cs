using System.Net;
using System.Net.Http.Json;
using Knot.API.Container;

namespace Knot.CTL.Services;

public class KnotHttpClient
{
  private readonly HttpClient httpClient = new()
  {
    BaseAddress = new Uri("http://localhost:5018/api"),
  };

  public async Task<ContainerResponse?> CreateContainer(CreateContainerRequest createContainerRequest)
  {
    var response = await httpClient.PostAsJsonAsync("containers", createContainerRequest, CancellationToken.None);

    if (response.StatusCode == HttpStatusCode.Created)
    {
      ContainerResponse? containerResponse = await response.Content.ReadFromJsonAsync<ContainerResponse>();

      return containerResponse;
    }

    return null;
  }

  public async Task<List<ContainerResponse>?> GetContainers()
  {
    var containerResponse = await httpClient.GetAsync("containers");

    if (containerResponse.StatusCode == HttpStatusCode.OK)
    {
      List<ContainerResponse>? containers = await containerResponse.Content.ReadFromJsonAsync<List<ContainerResponse>>();

      if (containers != null)
      {
        return containers;
      }
    }

    return null;
  }
}