using Microsoft.AspNetCore.Mvc;
using WisdomPetMedicine.Rescue.Api.ApplicationServices;
using WisdomPetMedicine.Rescue.Api.Commands;

namespace WisdomPetMedicine.Rescue.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AdopterController(AdopterApplicationService adopterApplicationService, ILogger<AdopterController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(CreateAdopterCommand command)
    {
        await adopterApplicationService.HandleCommandAsync(command);
        return Ok();
    }

    [HttpPost("requestAdoption")]
    public async Task<IActionResult> Put(RequestAdoptionCommand command)
    {
        await adopterApplicationService.HandleCommandAsync(command);
        return Ok();
    }

    [HttpPost("approveAdoption")]
    public async Task<IActionResult> Put(ApproveAdoptionCommand command)
    {
        await adopterApplicationService.HandleCommandAsync(command);
        return Ok();
    }

    [HttpPost("rejectAdoption")]
    public async Task<IActionResult> Put(RejectAdoptionCommand command)
    {
        await adopterApplicationService.HandleCommandAsync(command);
        return Ok();
    }

    [HttpPut("phonenumber")]
    public async Task<IActionResult> Put(SetAdopterPhoneNumberCommand command)
    {
        await adopterApplicationService.HandleCommandAsync(command);
        return Ok();
    }
}