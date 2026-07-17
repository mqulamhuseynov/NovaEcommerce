using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs.WishlistDtos;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/wishlists")]
    public class WishlistController(IWishlistService wishlistService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWishlists()
        {
            var result = await wishlistService.GetWishlists(GetUserId());
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistRequestDto request)
        {
            var result = await wishlistService.CreateWishlist(GetUserId(), request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetItems(int id)
        {
            var result = await wishlistService.GetWishlistItems(GetUserId(), id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/items")]
        public async Task<IActionResult> AddItem(int id, [FromBody] AddWishlistItemRequestDto request)
        {
            var result = await wishlistService.AddItem(GetUserId(), id, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}/items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int id, int itemId)
        {
            var result = await wishlistService.RemoveWishlistItem(GetUserId(), id, itemId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{id}/items/{itemId}/notify")]
        public async Task<IActionResult> RequestNotify(int id, int itemId)
        {
            var result = await wishlistService.RequestNotify(GetUserId(), id, itemId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/share")]
        public async Task<IActionResult> GetShareLink(int id)
        {
            var result = await wishlistService.ShareWishlistUrl(GetUserId(), id);
            return StatusCode(result.StatusCode, result);
        }
        //user id yoxlamag ucun
        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}