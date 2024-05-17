using Microsoft.AspNetCore.Mvc;

namespace Motto.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MotoController : ControllerBase
{
    private readonly CreateMotoUseCase createMotoUseCase;
    private readonly SelectMoto selectMotoUseCase;
    private readonly UpdateMoto updateMotoUseCase;

    public MotoController(CreateMotoUseCase createMotoUseCase, 
        SelectMoto selectMotoUseCase,
        UpdateMoto updateMotoUseCase)
    {
        this.createMotoUseCase = createMotoUseCase;
        this.selectMotoUseCase = selectMotoUseCase;
        this.updateMotoUseCase = updateMotoUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> Motos([FromQuery] MotoFilterRequest motoFilter)
    {
        var response = await selectMotoUseCase.Handle(motoFilter);

        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPost]
    public async Task<IActionResult> Motos([FromBody] CreateMotoRequest moto)
    {

        var response = await createMotoUseCase.Handle(moto);
     
        if (response.Success)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Message);
    }

    [HttpPut]
    public async Task<IActionResult> MotosPut(UpdateMotoRequest moto)
    {
        var response = await updateMotoUseCase.Handle(moto);

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
