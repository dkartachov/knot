using Knot.API.Container;
using Knot.API.Pod;
using Knot.Models;
using Knot.Services.ContainerRuntime;
using Knot.Services.Containers;
using Knot.Services.Pods;
using Microsoft.AspNetCore.Mvc;

namespace Knot.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PodsController(
  IContainerRuntime containerRuntime,
  IPodsService podsService) : ControllerBase
{
  private readonly IContainerRuntime containerRuntime = containerRuntime;
  private readonly IPodsService podsService = podsService;

  [HttpGet]
  [ProducesResponseType<IList<PodResponse>>(StatusCodes.Status200OK)]
  public async Task<IActionResult> GetPods()
  {
    var pods = await podsService.GetPods();
    var podsResponse = new List<PodResponse>();

    foreach (var pod in pods)
    {
      var containersResponse = new List<ContainerResponse>();

      foreach (var container in pod.Containers)
      {
        containersResponse.Add(new ContainerResponse(
          Id: container.Id,
          Name: container.Name,
          Image: container.Image,
          StartTime: container.StartTime
        ));
      }

      podsResponse.Add(new PodResponse(
        Id: pod.Id,
        Name: pod.Name,
        Containers: containersResponse
      ));
    }

    return Ok(podsResponse);
  }

  [HttpGet("{id:length(24)}")]
  [ProducesResponseType<PodResponse>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetPod(string id)
  {
    var pod = await podsService.GetPod(id);

    if (pod == null)
    {
      return NotFound();
    }

    var containersResponse = new List<ContainerResponse>();

    foreach (var container in pod.Containers)
    {
      containersResponse.Add(new ContainerResponse(
        Id: container.Id,
        Name: container.Name,
        Image: container.Image,
        StartTime: container.StartTime
      ));
    }

    var podResponse = new PodResponse(
      Id: pod.Id,
      Name: pod.Name,
      Containers: containersResponse
    );

    return Ok(podResponse);
  }

  [HttpPost]
  public async Task<IActionResult> CreatePod(CreatePodRequest createPodRequest)
  {
    var containers = new List<Container>();

    foreach (var createContainerRequest in createPodRequest.Containers)
    {
      StartContainerResponse response = await containerRuntime.StartContainer(createContainerRequest);

      var container = new Container(
        containerId: response.ContainerId,
        name: createContainerRequest.Name,
        image: createContainerRequest.Image,
        startTime: DateTime.UtcNow
      );

      containers.Add(container);
    }

    var pod = new Pod(
      name: createPodRequest.Name,
      containers: containers
    );

    await podsService.CreatePod(pod);

    var containersResponse = new List<ContainerResponse>();

    foreach (var container in pod.Containers)
    {
      containersResponse.Add(new ContainerResponse(
        Id: "",
        Name: container.Name,
        Image: container.Image,
        StartTime: container.StartTime
      ));
    }

    var podResponse = new PodResponse(
      Id: pod.Id,
      Name: pod.Name,
      Containers: containersResponse
    );

    return CreatedAtAction(nameof(GetPod), new { id = podResponse.Id }, podResponse);
  }
}