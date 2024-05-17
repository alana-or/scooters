using Microsoft.AspNetCore.Mvc;

namespace Motto.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MotoController : ControllerBase
{
    private readonly CreateMotoUseCase createMotoUseCase;
    private readonly SelectMotoUseCase selectMotoUseCase;
    private readonly UpdateMotoUseCase updateMotoUseCase;

    public MotoController(CreateMotoUseCase createMotoUseCase, 
        SelectMotoUseCase selectMotoUseCase,
        UpdateMotoUseCase updateMotoUseCase)
    {
        this.createMotoUseCase = createMotoUseCase;
        this.selectMotoUseCase = selectMotoUseCase;
        this.updateMotoUseCase = updateMotoUseCase;
    }

    [HttpGet]
    public IActionResult Motos([FromQuery] MotoFilterRequest motoFilter)
    {
        var response = selectMotoUseCase.Handle(motoFilter);

        if (response.Success)
        {
            return Ok(response.Message);
        }

        return BadRequest(response.Message);
    }

    [HttpPost]
    public IActionResult Motos([FromBody] CreateMotoRequest moto)
    {

        var response = createMotoUseCase.Handle(moto);
     
        if (response.Success)
        {
            return Ok(response.Message);
        }

        return BadRequest(response.Message);
    }

    [HttpPut]
    public IActionResult MotosPut(UpdateMotoRequest moto)
    {
        var response = updateMotoUseCase.Handle(moto);

        if (response.Success)
        {
            return Ok(response.Message);
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
