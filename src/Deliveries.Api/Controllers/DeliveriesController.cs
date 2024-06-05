using Deliveries.Application;
using Deliveries.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deliveries.Controllers;

[Route("v1/api/[controller]")]
[ApiController]
public class DeliveriesController(IDeliveriesService service) : ControllerBase
{
    private readonly IDeliveriesService _service = service;

    [HttpPost("rentals/create")]
    public async Task<IActionResult> CreateRentalAsync([FromBody] DeliveryPersonRentalCreateModel rental)
    {
        var response = await _service.CreateRentalAsync(rental);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpGet("rentals/{personId}")]
    public async Task<IActionResult> GetRentalsAsync(Guid personId)
    {
        var response = await _service.GetPersonRentalsAsync(personId);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpGet("scooters")]
    public async Task<IActionResult> GetScootersAsync()
    {
        var response = await _service.GetScootersAsync();

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPost("person/create")]
    public async Task<IActionResult> CreatePersonAsync([FromBody] DeliveryPersonCreateModel deliveryPerson)
    {
        var response = await _service.CreatePersonAsync(deliveryPerson);
     
        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("person/update")]
    public async Task<IActionResult> UpdatePersonAsync([FromBody] DeliveryPersonUpdateModel deliveryPerson)
    {
        var response = await _service.UpdatePersonAsync(deliveryPerson);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }
}
