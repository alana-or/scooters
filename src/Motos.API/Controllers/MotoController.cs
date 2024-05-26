using Microsoft.AspNetCore.Mvc;
using Motos.API.Models;
using Motos.API.Services;

namespace Motos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MotoController : ControllerBase
{
    private readonly IMotoService service;

    public MotoController(IMotoService service)
    {
        this.service = service;
    }

    [HttpGet("Motos2024")]
    public async Task<IActionResult> Motos2024()
    {
        var response = await service.GetTop5LatestMotosLogsAsync();

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpGet]
    public async Task<IActionResult> Motos([FromQuery] MotoFilterRequest motoFilter)
    {
        var response = await service.Get(motoFilter);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Motos([FromBody] CreateMotoRequest moto)
    {

        var response = await service.Create(moto);
     
        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPut]
    public async Task<IActionResult> MotosPut(UpdateMotoRequest moto)
    {
        var response = await service.Update(moto);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpDelete]
    public IActionResult MotosDelete(CreateMotoRequest moto)
    {
        //grava no banco

        return Ok();
    }
}
