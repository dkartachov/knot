using Knot.Models;
using Knot.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Knot.Services.Pods;

public class PodsServiceMongoDB : IPodsService
{
  private readonly IMongoCollection<Pod> _podCollection;

  public PodsServiceMongoDB(IOptions<MongoDBOptions> options)
  {
    var client = new MongoClient(options.Value.ConnectionString);
    var database = client.GetDatabase(options.Value.DatabaseName);

    _podCollection = database.GetCollection<Pod>(options.Value.PodsCollectionName);
  }

  public async Task CreatePod(Pod Pod)
  {
    await _podCollection.InsertOneAsync(Pod);
  }

  public async Task DeletePod(string id)
  {
    await _podCollection.DeleteOneAsync(p => p.Id == id);
  }

  public async Task<Pod> GetPod(string id)
  {
    return await _podCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
  }

  public async Task<IList<Pod>> GetPods()
  {
    return await _podCollection.Find(_ => true).ToListAsync();
  }
}