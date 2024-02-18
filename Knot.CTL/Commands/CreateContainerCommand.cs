using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Knot.API.Container;
using Knot.CTL.Services;

namespace Knot.CTL.Commands;

public class CreateContainerCommand : Command
{
  public CreateContainerCommand()
  {
    Name = "container";

    Flags().String("image", "i", "");
  }

  protected override bool ValidateArgs(string[] args)
  {
    if (args.Length == 0)
    {
      Console.WriteLine("container name required");
      return false;
    }

    if (args.Length > 1)
    {
      Console.WriteLine("expected 1 argument");
      return false;
    }

    return true;
  }

  protected override async Task RunAsync(string[] args)
  {
    string image = Flags().GetString("image");

    if (image.Length == 0)
    {
      Console.WriteLine("image required");
      return;
    }

    var createContainerRequest = new CreateContainerRequest
    (
      name: args[0],
      image: image
    );

    try
    {
      ContainerResponse? containerResponse = await new KnotHttpClient().CreateContainer(createContainerRequest);

      if (containerResponse == null)
      {
        Console.WriteLine("error creating container");
        return;
      }

      Console.WriteLine($"created and started container {containerResponse.Name}");
    }
    catch (HttpRequestException e)
    {
      Console.WriteLine(e.Message);
    }
  }
}