namespace Knot.CTL.Commands;

public class CreateContainerCommand : Command
{
  public CreateContainerCommand()
  {
    Name = "container";

    Flags().String("image", "", "");
    // Flags().Int("limit", "", 0);
  }

  protected override bool ValidateArgs(string[] args)
  {
    if (args.Length == 0)
    {
      Console.WriteLine("container name required");
      return false;
    }
    return true;
  }

  protected override void Run(string[] args)
  {
    string image = Flags().GetString("image");
    // int limit = Flags().GetInt("limit");

    Console.WriteLine($"args are: {string.Join(' ', args)}");
    Console.WriteLine($"image is: {image}");
    // Console.WriteLine($"limit is: {limit}");
  }
}