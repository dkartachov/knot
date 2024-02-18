namespace Knot.CTL.Commands;

public class CreateCommand : Command
{
  public CreateCommand()
  {
    Name = "create";

    AddCommand(new CreateContainerCommand());
  }
}