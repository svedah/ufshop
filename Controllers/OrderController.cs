using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ufshop.Controllers;

[ApiController]
// [Route("api/[controller]")]
public class OrderController : ControllerBase
{
    readonly Guid EMPTY = Guid.Empty;
    [HttpGet]
    [Route("[controller]/swishcb/{id?}")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult SwishCB(Guid id)//WARNING: Guid expects dashes in the uri string
    {
        if (id == Guid.Empty)
        {
            return new NotFoundResult();
        }
        return new OkObjectResult(id.ToString()); //input);
    }
}