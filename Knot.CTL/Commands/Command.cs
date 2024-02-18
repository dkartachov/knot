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
    List<string> commandArgsList = [];

    for (int i = 0; i < args.Length; i++)
    {
      string arg = args[i];

      // TODO Maybe use regex? Right now any number of dashes will register as a flag e.g.: ------image
      if (arg.StartsWith('-'))
      {
        string flag = arg.TrimStart('-');

        // TODO support alias
        if (_FlagSet.Exists(flag))
        {
          switch (_FlagSet.GetFlag(flag).Type)
          {
            case Flag.TYPE.STRING:
              if (i < args.Length - 1)
              {
                _FlagSet.SetString(flag, args[i + 1]);
                i++;
              }

              break;
            case Flag.TYPE.INT:
              if (i < args.Length - 1 && int.TryParse(args[i + 1], out int intValue))
              {
                _FlagSet.SetInt(flag, intValue);
                i++;
              }
              else
              {
                return;
              }

              break;
            case Flag.TYPE.BOOL:
              if (i + 1 < args.Length && bool.TryParse(args[i + 1], out bool boolValue))
              {
                _FlagSet.SetBool(flag, boolValue);
                i++;
              }
              else
              {
                _FlagSet.SetBool(flag, true);
              }

              break;
          }
        }
        else
        {
          // TODO print help for command
          Console.WriteLine($"flag {flag} not supported for {Name}");
          return;
        }
      }
      else if (Commands.Count > 0)
      {
        if (Commands.Exists((c) => c.Name == arg || c.Alias == arg))
        {
          Command cmd = Commands.First((c) => c.Name == arg || c.Alias == arg);

          cmd.Execute(args.Skip(i + 1).ToArray());

          break;
        }
        else
        {
          Console.WriteLine($"subcommand {arg} not found for {Name}");
          return;
        }
      }
      else
      {
        commandArgsList.Add(arg);
      }
    }

    string[] commandArgs = [.. commandArgsList];

    if (ValidateArgs(commandArgs))
    {
      // TODO find a way to run one or the other based on which method is implemented in derived class
      Run(commandArgs);
      RunAsync(commandArgs).Wait();
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
  protected virtual Task RunAsync(string[] args) { return Task.CompletedTask; }
}
