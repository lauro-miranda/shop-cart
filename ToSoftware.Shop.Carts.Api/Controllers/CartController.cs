using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ToSoftware.Shop.Carts.Api.Domain.Services.Contracts;
using ToSoftware.Shop.Carts.Api.Messages;

namespace ToSoftware.Shop.Carts.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class CartController : Controller
    {
        ICartService CartService { get; }

        public CartController(ICartService cartService)
        {
            CartService = cartService;
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] CartRequestMessage requestMessage)
            => await WithResponseAsync(() => CartService.CreateAsync(requestMessage));

        [HttpPut, Route("{code}")]
        public async Task<IActionResult> CreateAsync(Guid code, [FromBody] CartRequestMessage requestMessage)
            => await WithResponseAsync(() => CartService.UpdateAsync(code, requestMessage));

        [HttpGet, Route("{code}")]
        public async Task<IActionResult> FindAsync(Guid code)
            => await WithResponseAsync(() => CartService.FindAsync(code));
    }
}