using Knot.API.Container;

namespace Knot.API.Pod;

public record CreatePodRequest(
  string Name,
  IList<CreateContainerRequest> Containers
);