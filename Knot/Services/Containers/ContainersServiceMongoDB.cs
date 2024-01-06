using Knot.Models;
using Knot.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Knot.Services.Containers;

public class ContainersServiceMongoDB : IContainersService
{
  private readonly IMongoCollection<Container> _containerCollection;

  public ContainersServiceMongoDB(IOptions<MongoDBOptions> options)
  {
    var client = new MongoClient(options.Value.ConnectionString);
    var database = client.GetDatabase(options.Value.DatabaseName);

    _containerCollection = database.GetCollection<Container>(options.Value.ContainersCollectionName);
  }

  public async Task CreateContainer(Container container)
  {
    await _containerCollection.InsertOneAsync(container);
  }

  public async Task DeleteContainer(string id)
  {
    await _containerCollection.DeleteOneAsync(c => c.Id == id);
  }

  public async Task<Container> GetContainer(string id)
  {
    return await _containerCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
  }

  public async Task<IList<Container>> GetContainers()
  {
    return await _containerCollection.Find(_ => true).ToListAsync();
  }
}