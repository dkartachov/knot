using Knot.API.Container;

namespace Knot.API.Pod;

public record PodResponse(
  Guid Guid,
  string Name,
  IList<ContainerResponse> Containers
);