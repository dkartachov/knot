using System.ComponentModel;

namespace Knot.API.Container;

public record CreateContainerRequest
{
  [DefaultValue("container-1")]
  public string Name { get; set; }
  [DefaultValue("strm/helloworld-http")]
  public string Image { get; set; }

  public CreateContainerRequest(string name, string image)
  {
    Name = name;
    Image = image;
  }
}