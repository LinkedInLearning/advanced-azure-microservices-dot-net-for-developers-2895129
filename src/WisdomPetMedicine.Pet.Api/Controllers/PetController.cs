using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WisdomPetMedicine.Pet.Api.ApplicationServices;
using WisdomPetMedicine.Pet.Api.Commands;

namespace WisdomPetMedicine.Pet.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetController : ControllerBase
    {
        private readonly PetApplicationService petApplicationService;
        private readonly ILogger<PetController> logger;

        public PetController(PetApplicationService petApplicationService,
                             ILogger<PetController> logger)
        {
            this.petApplicationService = petApplicationService;
            this.logger = logger;
        }

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
}