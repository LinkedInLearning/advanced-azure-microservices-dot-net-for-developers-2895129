using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WisdomPetMedicine.Rescue.Api.ApplicationServices;
using WisdomPetMedicine.Rescue.Api.Commands;

namespace WisdomPetMedicine.Rescue.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdopterController : ControllerBase
    {
        private readonly AdopterApplicationService adopterApplicationService;
        private readonly ILogger<AdopterController> logger;

        public AdopterController(AdopterApplicationService adopterApplicationService, ILogger<AdopterController> logger)
        {
            this.adopterApplicationService = adopterApplicationService;
            this.logger = logger;
        }

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
    }
}