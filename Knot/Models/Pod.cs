using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Knot.Models;

public class Pod
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public string Id { get; set; } = string.Empty;
  public string Name { get; set; }
  public IList<Container> Containers { get; set; }

  public Pod(
    string name,
    IList<Container> containers)
  {
    Name = name;
    Containers = containers;
  }
}