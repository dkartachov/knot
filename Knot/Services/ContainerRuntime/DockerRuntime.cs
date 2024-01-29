using System.Runtime.CompilerServices;
using System.Text;
using Docker.DotNet;
using Docker.DotNet.Models;
using Knot.API.Container;
using Knot.API.ContainerRuntime;

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

  public async IAsyncEnumerable<string> StreamContainerLogs(
    string containerId,
    StreamContainerLogsRequest streamContainerLogsRequest,
    [EnumeratorCancellation] CancellationToken cancellationToken)
  {
    var containerLogsParameters = new ContainerLogsParameters
    {
      Tail = streamContainerLogsRequest.Tail,
      ShowStdout = streamContainerLogsRequest.ShowStdout,
      ShowStderr = streamContainerLogsRequest.ShowStderr,
      Follow = streamContainerLogsRequest.Follow
    };

    using var multiplexedStream = await client.Containers.GetContainerLogsAsync(containerId, false, containerLogsParameters, cancellationToken);

    while (true)
    {
      cancellationToken.ThrowIfCancellationRequested();

      byte[] buffer = new byte[4096];
      var result = await multiplexedStream.ReadOutputAsync(buffer, 0, buffer.Length, cancellationToken);

      if (result.EOF) yield break;

      var logLine = Encoding.UTF8.GetString(buffer).TrimEnd('\0');

      yield return logLine;
    }
  }
}