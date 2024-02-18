
namespace Knot.CTL.Commands;

public class GetCommand : Command
{
  public GetCommand()
  {
    Name = "get";

    AddCommand(new GetContainersCommand());
  }
}