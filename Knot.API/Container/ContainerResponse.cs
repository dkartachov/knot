namespace Knot.API.Container;

public record ContainerResponse(
  string Id,
  string Name,
  string Image,
  DateTime StartTime
);