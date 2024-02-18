
using Knot.CTL.Services;

namespace Knot.CTL.Commands;

public class GetContainersCommand : Command
{
  public GetContainersCommand()
  {
    Name = "containers";
  }

  protected override async Task RunAsync(string[] args)
  {
    var containers = await new KnotHttpClient().GetContainers();
  }
}