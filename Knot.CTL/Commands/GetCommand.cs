namespace Knot.CTL.Commands;

public class GetCommand : Command
{
  public GetCommand()
  {
    Name = "get";
  }

  protected override void Run(string[] args)
  {
    Array.ForEach(args, Console.WriteLine);
  }
}