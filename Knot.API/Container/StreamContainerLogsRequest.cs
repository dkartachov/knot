namespace Knot.API.ContainerRuntime;

public class StreamContainerLogsRequest
{
  public string Tail { get; set; } = "50";
  public bool? ShowStdout { get; set; }
  public bool? ShowStderr { get; set; }
  public bool? Follow { get; set; }
}