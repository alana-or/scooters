using Microsoft.AspNetCore.Mvc;
using Scooters.Api.Models;
using Scooters.Api.Services;

namespace Scooters.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScootersController(IScootersService service) : ControllerBase
{
    private readonly IScootersService _service = service;

    [HttpGet("scooters2024")]
    public async Task<IActionResult> GetScooters2024()
    {
        var response = await _service.GetTop5LatestMotosLogsAsync();

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpGet("scooters")]
    public async Task<IActionResult> Get([FromQuery] ScooterFilter scooterFilter)
    {
        var response = await _service.GetAsync(scooterFilter);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ScooterCreate scooter)
    {

        var response = await _service.CreateAsync(scooter);
     
        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(ScooterUpdate scooter)
    {
        var response = await _service.UpdateAsync(scooter);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpDelete("delete")]
    public IActionResult Delete(ScooterCreate scooter)
    {
        //grava no banco

        return Ok();
    }
}
