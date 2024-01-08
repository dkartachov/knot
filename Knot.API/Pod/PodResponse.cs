using Knot.API.Container;

namespace Knot.API.Pod;

public record PodResponse(
  string Id,
  string Name,
  IList<ContainerResponse> Containers
);