namespace Knot.Options;

public class MongoDBOptions
{
  public const string MongoDB = "MongoDB";
  public string ConnectionString { get; set; } = string.Empty;
  public string DatabaseName { get; set; } = string.Empty;
  public string ContainersCollectionName { get; set; } = string.Empty;
}