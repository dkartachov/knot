using Knot.API.Container;
using Knot.API.ContainerRuntime;

namespace Knot.Services.ContainerRuntime;

public interface IContainerRuntime
{
  public Task<StartContainerResponse> StartContainer(CreateContainerRequest createContainerRequest);

  public Task<bool> StopContainer(string containerId);

  public IAsyncEnumerable<string> StreamContainerLogs(
    string containerId,
    StreamContainerLogsRequest streamContainerLogsRequest,
    CancellationToken cancellationToken);
}

// CHECKME move these somewhere else?
public class StartContainerResponse(string containerId, bool started)
{
  public string ContainerId { get; } = containerId;
  public bool Started { get; } = started;
};