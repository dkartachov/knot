namespace Knot.CTL.Commands;

public class RootCommand : Command
{
  public RootCommand()
  {
    Name = "knotctl";

    var getCmd = new GetCommand();
    var createCmd = new CreateCommand();

    AddCommand(getCmd);
    AddCommand(createCmd);
  }
}