using Microsoft.AspNetCore.Mvc;
using WisdomPetMedicine.Hospital.Api.ApplicationServices;
using WisdomPetMedicine.Hospital.Api.Commands;

namespace WisdomPetMedicine.Hospital.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController(HospitalApplicationService applicationService,
                         ILogger<PatientController> logger) : ControllerBase
{
    [HttpPut("weight")]
    public async Task<IActionResult> Put(SetWeightCommand command)
    {
        try
        {
            await applicationService.HandleAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger?.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("bloodType")]
    public async Task<IActionResult> Put(SetBloodTypeCommand command)
    {
        try
        {
            await applicationService.HandleAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger?.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("admit")]
    public async Task<IActionResult> Post(AdmitPatientCommand command)
    {
        try
        {
            await applicationService.HandleAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger?.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("discharge")]
    public async Task<IActionResult> Post(DischargePatientCommand command)
    {
        try
        {
            await applicationService.HandleAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger?.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("procedure")]
    public async Task<IActionResult> Post(AddProcedureCommand command)
    {
        try
        {
            await applicationService.HandleAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            logger?.LogError(ex.Message);
            return BadRequest(ex.Message);
        }
    }
}