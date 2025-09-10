using Microsoft.AspNetCore.Mvc;
using WisdomPetMedicine.Pet.Api.ApplicationServices;
using WisdomPetMedicine.Pet.Api.Commands;

namespace WisdomPetMedicine.Pet.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PetController(PetApplicationService petApplicationService,
                     ILogger<PetController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(CreatePetCommand command)
    {
        try
        {
            await petApplicationService.HandleCommandAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("name")]
    public async Task<IActionResult> Put(SetNameCommand command)
    {
        try
        {
            await petApplicationService.HandleCommandAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("breed")]
    public async Task<IActionResult> Put(SetBreedCommand command)
    {
        try
        {
            await petApplicationService.HandleCommandAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("color")]
    public async Task<IActionResult> Put(SetColorCommand command)
    {
        try
        {
            await petApplicationService.HandleCommandAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("dateofbirth")]
    public async Task<IActionResult> Put(SetDateOfBirthCommand command)
    {
        try
        {
            await petApplicationService.HandleCommandAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("flagforadoption")]
    public async Task<IActionResult> Post(FlagForAdoptionCommand command)
    {
        try
        {
            await petApplicationService.HandleCommandAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("transfertohospital")]
    public async Task<IActionResult> Post(TransferToHospitalCommand command)
    {
        try
        {
            await petApplicationService.HandleCommandAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
}