using Deliveries.Api.Models;
using Deliveries.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Deliveries.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
public class DeliveriesController(IDeliveriesService service) : ControllerBase
{
    private readonly IDeliveriesService _service = service;

    [HttpPost("rentals/create")]
    public async Task<IActionResult> CreateRental([FromBody] RentalCreate rental)
    {
        var response = await _service.CreateRentalAsync(rental);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpGet("rentals")]
    public async Task<IActionResult> GetRentals([FromBody] Guid personId)
    {
        var response = await _service.GetRentalsAsync(personId);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpGet("scooters")]
    public async Task<IActionResult> GetScooters()
    {
        var response = await _service.GetScootersAsync();

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] DeliveryPersonCreate deliveryPerson)
    {
        var response = await _service.CreatePersonAsync(deliveryPerson);
     
        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("Put")]
    public async Task<IActionResult> Put([FromBody] DeliveryPersonUpdate deliveryPerson)
    {
        var response = await _service.UpdatePersonAsync(deliveryPerson);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpDelete("Delete")]
    public IActionResult Delete(DeliveryPersonCreate deliveryPerson)
    {
        //grava no banco

        return Ok();
    }
}
