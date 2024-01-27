using Docker.DotNet;
using Knot.API.Container;

namespace Knot.Services.ContainerRuntime;

public interface IContainerRuntime
{
  public Task<StartContainerResponse> StartContainer(CreateContainerRequest createContainerRequest);
  public Task<bool> StopContainer(string id);
  public Task<MultiplexedStream> ContainerLogs(string id);
}

// CHECKME move this somewhere else?
public class StartContainerResponse(string containerId, bool started)
{
  public string ContainerId { get; } = containerId;
  public bool Started { get; } = started;
};