namespace Knot.CTL.Commands;

public class RootCommand : Command
{
  public RootCommand()
  {
    Name = "knotctl";

    var getCmd = new GetCommand();
    var createCmd = new CreateCommand();
    var createContainerCmd = new CreateContainerCommand();

    createCmd.AddCommand(createContainerCmd);

    AddCommand(getCmd);
    AddCommand(createCmd);
  }
}