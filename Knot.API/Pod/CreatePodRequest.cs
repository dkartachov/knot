using System.ComponentModel;
using Knot.API.Container;

namespace Knot.API.Pod;

public class CreatePodRequest
{
  [DefaultValue("pod-1")]
  public string Name { get; set; }
  public IList<CreateContainerRequest> Containers { get; set; }

  public CreatePodRequest() { }

  public CreatePodRequest(string name, IList<CreateContainerRequest> createContainerRequests)
  {
    Name = name;
    Containers = createContainerRequests;
  }
}