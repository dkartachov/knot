using Docker.DotNet;
using Docker.DotNet.Models;
using Knot.API.Container;

namespace Knot.Services.ContainerRuntime;

public class DockerRuntime : IContainerRuntime
{
  // TODO add configuration options
  private readonly DockerClient client = new DockerClientConfiguration().CreateClient();

  public async Task<StartContainerResponse> StartContainer(CreateContainerRequest createContainerRequest)
  {
    // TODO handle exceptions
    // 1. Invalid container name
    // 2. Container name exists
    var createContainerResponse = await client.Containers.CreateContainerAsync(new CreateContainerParameters
    {
      Name = createContainerRequest.Name,
      Image = createContainerRequest.Image,
      HostConfig = new HostConfig
      {
        PublishAllPorts = true
      },
      AttachStdin = true,
      AttachStdout = true,
    });

    bool started = await client.Containers.StartContainerAsync(createContainerResponse.ID, new ContainerStartParameters());

    return new StartContainerResponse(
      containerId: createContainerResponse.ID,
      started: started
    );
  }

  public async Task<bool> StopContainer(string id)
  {
    try
    {
      await client.Containers.RemoveContainerAsync(id, new ContainerRemoveParameters
      {
        Force = true
      });
    }
    catch (DockerApiException)
    {
      return false;
    }

    return true;
  }
}