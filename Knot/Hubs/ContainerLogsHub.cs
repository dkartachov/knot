using System.Text;
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

  public async Task SendContainerLogs(string id)
  {
    try
    {
      using var stream = await containerRuntime.ContainerLogs(id);

      while (true)
      {
        // this should be more than enough for each log (?)
        byte[] buffer = new byte[1024];
        var result = await stream.ReadOutputAsync(buffer, 0, buffer.Length, Context.ConnectionAborted);

        if (result.EOF) return;

        string str = Encoding.Default.GetString(buffer).TrimEnd('\0');

        Console.Write(str);

        await Clients.Client(Context.ConnectionId).SendAsync("test", $"{str}");
      }

    }
    catch (OperationCanceledException)
    {
      logger.LogInformation("Client {id} canceled container logs stream", Context.ConnectionId);
    }
  }
}