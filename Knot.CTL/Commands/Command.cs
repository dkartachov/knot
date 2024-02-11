using Knot.CTL.Flags;

namespace Knot.CTL.Commands;

public abstract class Command
{
  protected string? Name { get; set; }
  protected string? Alias { get; set; }
  private readonly List<Command> Commands = [];
  private readonly FlagSet _FlagSet = new();
  private Command? Parent { get; set; }

  public void Execute(string[] args)
  {
    Walk(args);

    string[] commandArgs = ParseArgs(args);

    if (ValidateArgs(commandArgs))
    {
      Run(commandArgs);
    }
  }

  public void AddCommand(Command command)
  {
    command.Parent = this;
    Commands.Add(command);
  }

  protected FlagSet Flags()
  {
    return _FlagSet;
  }

  protected virtual bool ValidateArgs(string[] args) { return true; }
  protected virtual void Run(string[] args) { }

  /// <summary>
  /// Walks through arguments to find subcommands and executes the subcommands
  /// </summary>
  /// <param name="args"></param>
  private void Walk(string[] args)
  {
    if (args.Length > 0)
    {
      string arg = args[0];

      // Check if arg is a sub command
      foreach (Command cmd in Commands)
      {
        if (cmd.Name == arg || cmd.Alias == arg)
        {
          cmd.Execute(args.Skip(1).ToArray());

          return;
        }
      }
    }
  }

  /// <summary>
  /// Parses args to extract command arguments and flags. 
  /// Saves flag values to flagset and returns command arguments
  /// </summary>
  /// <param name="args"></param>
  /// <returns>command arguments</returns>
  private string[] ParseArgs(string[] args)
  {
    // TODO override flags
    List<string> commandArgsList = [];

    for (int i = 0; i < args.Length; i++)
    {
      string arg = args[i];

      if (arg.StartsWith('-'))
      {
        string flag = arg.TrimStart('-');

        // TODO Support bool flags (?)
        if (i < args.Length - 1 && _FlagSet.Exists(flag))
        {
          Flag f = _FlagSet.GetFlag(flag);

          switch (f.Type)
          {
            case Flag.TYPE.STRING:
              _FlagSet.SetString(flag, args[i + 1]);
              break;
            case Flag.TYPE.INT:
              _FlagSet.SetInt(flag, int.Parse(args[i + 1]));
              break;
            case Flag.TYPE.BOOL:
              break;
          }

          // advance to prevent adding flag parameter to command args list
          i++;
        }
      }
      else
      {
        commandArgsList.Add(arg);
      }
    }

    return [.. commandArgsList];
  }
}
