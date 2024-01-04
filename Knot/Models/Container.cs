namespace Knot.Models;

public class Container
{
  public Guid Id { get; }
  public string ContainerId { get; }
  public string Name { get; }
  public string Image { get; }
  public DateTime StartTime { get; }

  public Container(
    Guid id,
    string containerId,
    string name,
    string image,
    DateTime startTime)
  {
    Id = id;
    ContainerId = containerId;
    Name = name;
    Image = image;
    StartTime = startTime;
  }
}