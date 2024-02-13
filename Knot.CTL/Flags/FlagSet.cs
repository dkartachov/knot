namespace Knot.CTL.Flags;

public class FlagSet
{
  private readonly Dictionary<string, Flag> Flags = [];

  public void String(string name, string alias, string value)
  {
    Flags.Add(name, new Flag
    {
      Name = name,
      Value = value,
      Alias = alias,
      Type = Flag.TYPE.STRING,
    });
  }

  public void Int(string name, string alias, int value)
  {
    Flags.Add(name, new Flag
    {
      Name = name,
      Alias = alias,
      Value = value.ToString(),
      Type = Flag.TYPE.INT,
    });
  }

  public void Bool(string name, string alias, bool value)
  {
    Flags.Add(name, new Flag
    {
      Name = name,
      Alias = alias,
      Value = value.ToString(),
      Type = Flag.TYPE.BOOL,
    });
  }

  public Flag GetFlag(string name)
  {
    return Flags[name];
  }

  public void SetString(string name, string value)
  {
    if (Flags.TryGetValue(name, out Flag flag) && flag.Type == Flag.TYPE.STRING)
    {
      flag.Value = value;

      Flags[name] = flag;
    }
  }

  public string GetString(string name)
  {
    if (Flags.TryGetValue(name, out Flag value) && value.Value != null)
    {
      return value.Value;
    }

    return "";
  }

  public void SetInt(string name, int value)
  {
    if (Flags.TryGetValue(name, out Flag flag) && flag.Type == Flag.TYPE.INT)
    {
      flag.Value = value.ToString();

      Flags[name] = flag;
    }
  }

  public int GetInt(string name)
  {
    return Flags.TryGetValue(name, out Flag value) ? int.Parse(value.Value) : 0;
  }

  public void SetBool(string flag, bool value)
  {
    if (Flags.TryGetValue(flag, out Flag f) && f.Type == Flag.TYPE.BOOL)
    {
      f.Value = value.ToString();

      Flags[flag] = f;
    }
  }

  public bool GetBool(string name)
  {
    return Flags.TryGetValue(name, out Flag value) && bool.Parse(value.Value);
  }

  public bool Exists(string flag)
  {
    foreach (KeyValuePair<string, Flag> entry in Flags)
    {
      if (entry.Key == flag || entry.Value.Alias == flag) return true;
    }

    return false;
  }
}