using Microsoft.AspNetCore.Mvc;

namespace UrlMin.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class MyControllerBase : ControllerBase
{
    protected IActionResult HandleResponse<T>(T data) where T : class
        => data is not null ? Ok(data) : NotFound();
}
