using Microsoft.AspNetCore.Mvc;

namespace ToSoftware.Shop.Carts.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class MeController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Get() => Ok(new { name = "shop carts", version = "1.0" });
    }
}