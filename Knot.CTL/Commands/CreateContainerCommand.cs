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
    return true;
  }

  protected override void Run(string[] args)
  {
    string image = Flags().GetString("image");

    Console.WriteLine($"args are: {string.Join(' ', args)}");
    Console.WriteLine($"image is: {image}");
  }
}