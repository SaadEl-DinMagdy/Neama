using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Shared.Dtos;
using System.Security.Claims;

namespace Neama.Api.Controllers
{
    [Authorize] 

    public class AddressesController : BaseApiController
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost]
        public async Task<ActionResult<ReturnAddress>> CreateAddress(AddressCreateDto addressDto)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _addressService.SaveAddressAsync(userId, addressDto);

            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReturnAddress>>> GetAllAddresses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _addressService.GetAllAddressesAsync(userId);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnAddress>> GetAddressById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var result = await _addressService.GetAddressByIdAsync(userId, id);

            if (result == null) return NotFound(new ApiResponse(404));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            await _addressService.RemoveAddressAsync(id);

            return Ok();
        }
    }
}
