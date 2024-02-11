namespace Knot.CTL.Flags;

public struct Flag
{
  public enum TYPE
  {
    STRING,
    INT,
    BOOL
  }

  public string Name { get; set; }
  public string Alias { get; set; }
  public string Value { get; set; }
  public TYPE Type { get; set; }
}