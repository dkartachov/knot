using Knot.Models;

namespace Knot.Services.Containers;

public interface IContainersService
{
  public Task<IList<Container>> GetContainers();
  public Task<Container> GetContainer(string id);
  public Task CreateContainer(Container container);
  public Task DeleteContainer(string id);
}