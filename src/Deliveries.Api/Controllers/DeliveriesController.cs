using Deliveries.Api.Models;
using Deliveries.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Deliveries.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveriesController(IDeliveriesService service) : ControllerBase
{
    private readonly IDeliveriesService _service = service;

    [HttpGet("delivery_people")]
    public async Task<IActionResult> Get()
    {
        var response = await _service.GetAsync();

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] DeliveryPersonCreate deliveryPerson)
    {
        var response = await _service.CreateAsync(deliveryPerson);
     
        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("Put")]
    public async Task<IActionResult> Put([FromBody] DeliveryPersonUpdate deliveryPerson)
    {
        var response = await _service.UpdateAsync(deliveryPerson);

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
