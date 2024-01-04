using Knot.Services.ContainerRuntime;
using Knot.Models;
using Knot.API.Container;
using Microsoft.AspNetCore.Mvc;

namespace Knot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContainersController(IContainerRuntime containerRuntime) : ControllerBase
{
  private readonly IContainerRuntime containerRuntime = containerRuntime;

  [HttpGet]
  public IActionResult GetContainers()
  {
    return Ok();
  }

  [HttpGet("{id:guid}")]
  [ProducesResponseType<ContainerResponse>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult GetContainer(Guid id)
  {
    return Ok(id);
  }

  [HttpPost]
  public async Task<IActionResult> CreateContainer(CreateContainerRequest createContainerRequest)
  {
    StartContainerResponse resp = await containerRuntime.StartContainer(createContainerRequest);

    var container = new Container(
      id: Guid.NewGuid(),
      containerId: resp.ContainerId,
      name: createContainerRequest.Name,
      image: createContainerRequest.Image,
      startTime: DateTime.UtcNow
    );

    // TODO save container to database

    var response = new ContainerResponse(
      Id: container.Id,
      Name: container.Name,
      Image: container.Image,
      StartTime: container.StartTime
    );

    return CreatedAtAction(nameof(GetContainer), new { id = response.Id }, response);
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteContainer(string id)
  {
    // Get container from database, if doesn't exist return NotFound()
    bool container = true;

    if (!container)
    {
      return NotFound();
    }

    // Extract container ID from container

    // Kill container
    bool stopped = await containerRuntime.StopContainer(id);

    if (!stopped)
    {
      return StatusCode(StatusCodes.Status500InternalServerError);
    }

    return NoContent();
  }
}