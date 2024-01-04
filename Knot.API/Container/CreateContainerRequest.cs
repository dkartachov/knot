namespace Knot.API.Container;

public record CreateContainerRequest(
  string Name,
  string Image
);