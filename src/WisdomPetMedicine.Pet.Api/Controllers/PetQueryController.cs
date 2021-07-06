using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Pet.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetQueryController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public PetQueryController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string query = @"SELECT p.Name_Value as Name,
                            p.Breed_Value as Breed,
                            Sex = 
                            CASE p.SexOfPet_Value
                              WHEN 0 THEN 'Male'
                              WHEN 1 THEN 'Female'
                            END,
                            p.Color_Value as Color,
                            p.DateOfBirth_Value as DateOfBirth,
                            p.Species_Value as Species
                            FROM Pets p";
            using var connection = new SqlConnection(configuration.GetConnectionString("Pet"));
            var orderDetail = (await connection.QueryAsync(query)).ToList();
            return Ok(orderDetail);
        }
    }
}