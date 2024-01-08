using Knot.Services.ContainerRuntime;
using Knot.Models;
using Knot.API.Container;
using Microsoft.AspNetCore.Mvc;
using Knot.Services.Containers;

namespace Knot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContainersController(
  IContainerRuntime containerRuntime,
  IContainersService containersService) : ControllerBase
{
  private readonly IContainerRuntime containerRuntime = containerRuntime;
  private readonly IContainersService containersService = containersService;

  [HttpGet]
  [ProducesResponseType<IList<ContainerResponse>>(StatusCodes.Status200OK)]
  public async Task<IActionResult> GetContainers()
  {
    var containers = await containersService.GetContainers();
    var containersResponse = new List<ContainerResponse>();

    foreach (var container in containers)
    {
      containersResponse.Add(new ContainerResponse(
        Id: container.Id,
        Name: container.Name,
        Image: container.Image,
        StartTime: container.StartTime
      ));
    }

    return Ok(containersResponse);
  }

  [HttpGet("{id:length(24)}")]
  [ProducesResponseType<ContainerResponse>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetContainer(string id)
  {
    var container = await containersService.GetContainer(id);

    if (container == null)
    {
      return NotFound();
    }

    var containerResponse = new ContainerResponse(
      Id: container.Id,
      Name: container.Name,
      Image: container.Image,
      StartTime: container.StartTime
    );

    return Ok(containerResponse);
  }

  [HttpPost]
  public async Task<IActionResult> CreateContainer(CreateContainerRequest createContainerRequest)
  {
    StartContainerResponse resp = await containerRuntime.StartContainer(createContainerRequest);

    var container = new Container(
      containerId: resp.ContainerId,
      name: createContainerRequest.Name,
      image: createContainerRequest.Image,
      startTime: DateTime.UtcNow
    );

    await containersService.CreateContainer(container);

    var response = new ContainerResponse(
      Id: container.Id,
      Name: container.Name,
      Image: container.Image,
      StartTime: container.StartTime
    );

    return CreatedAtAction(nameof(GetContainer), new { id = response.Id }, response);
  }

  [HttpDelete("{id:length(24)}")]
  public async Task<IActionResult> DeleteContainer(string id)
  {
    // Get container from database, if doesn't exist return NotFound()
    var container = await containersService.GetContainer(id);

    if (container == null)
    {
      return NotFound();
    }

    bool stopped = await containerRuntime.StopContainer(container.ContainerId);

    if (!stopped)
    {
      return StatusCode(StatusCodes.Status500InternalServerError);
    }

    await containersService.DeleteContainer(id);

    return NoContent();
  }
}