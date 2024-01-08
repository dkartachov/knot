using Knot.Models;

namespace Knot.Services.Pods;

public interface IPodsService
{
  public Task<IList<Pod>> GetPods();
  public Task<Pod> GetPod(string id);
  public Task CreatePod(Pod Pod);
  public Task DeletePod(string id);
}