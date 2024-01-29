using System.Runtime.CompilerServices;
using Knot.API.ContainerRuntime;
using Knot.Services.ContainerRuntime;
using Microsoft.AspNetCore.SignalR;

namespace Knot.Hubs;

public class ContainerLogsHub(
  IContainerRuntime containerRuntime,
  ILogger<ContainerLogsHub> logger) : Hub
{
  private readonly IContainerRuntime containerRuntime = containerRuntime;
  private readonly ILogger logger = logger;

  public override async Task OnConnectedAsync()
  {
    await base.OnConnectedAsync();

    logger.LogInformation("Client {id} connected", Context.ConnectionId);
  }

  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    await base.OnDisconnectedAsync(exception);

    logger.LogInformation("Client {id} disconnected", Context.ConnectionId);
  }

  public async IAsyncEnumerable<string> StreamContainerLogs(
    string containerId,
    StreamContainerLogsRequest streamContainerLogsRequest,
    [EnumeratorCancellation]
    CancellationToken cancellationToken)
  {
    await foreach (string log in containerRuntime.StreamContainerLogs(
      containerId,
      streamContainerLogsRequest,
      cancellationToken))
    {
      yield return log;
    }
  }
}