namespace Knot.API.Container;

public record ContainerResponse(
  Guid Id,
  string Name,
  string Image,
  DateTime StartTime
);