using Knot.API.Pod;
using Microsoft.AspNetCore.Mvc;

namespace Knot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PodsController : ControllerBase
{
  [HttpGet]
  public IActionResult GetPods()
  {
    return Ok();
  }

  [HttpGet("{id:guid}")]
  [ProducesResponseType<PodResponse>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult GetPod(Guid id)
  {
    return Ok(id);
  }

  [HttpPost]
  public IActionResult CreatePod(CreatePodRequest createPodRequest)
  {
    return Ok(createPodRequest);
  }
}