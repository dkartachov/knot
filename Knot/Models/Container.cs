using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Knot.Models;

public class Container
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;
  public string ContainerId { get; set; }
  public string Name { get; set; }
  public string Image { get; set; }
  public DateTime StartTime { get; set; }

  public Container(
    string containerId,
    string name,
    string image,
    DateTime startTime)
  {
    ContainerId = containerId;
    Name = name;
    Image = image;
    StartTime = startTime;
  }
}