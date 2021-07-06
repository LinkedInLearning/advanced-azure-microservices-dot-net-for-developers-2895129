using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WisdomPetMedicine.RescueQuery.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RescueQueryController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public RescueQueryController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = @"SELECT 
                        ram.Id, 
                        ram.Name,
                        ram.Breed,
                        Sex = 
                        CASE ram.Sex
	                        WHEN 0 THEN 'Male'
	                        WHEN 1 THEN 'Female'
                        END,
                        ra.AdopterId_Value AS AdopterId, 
                        AdoptionStatus =
                        CASE ra.AdoptionStatus
	                        WHEN 0 THEN 'None'
	                        WHEN 1 THEN 'Pending review'
	                        WHEN 2 THEN 'Accepted'
	                        WHEN 3 THEN 'Rejected'
                        END,
                        a.Name_Value as AdopterName, 
                        a.Questionnaire_DoYouRent,
                        a.Questionnaire_HasChildren,
                        a.Questionnaire_HasFencedYard,
                        a.Questionnaire_IsActivePerson,
                        a.Address_Street,
                        a.Address_Number,
                        a.Address_City,
                        a.Address_PostalCode,
                        a.Address_Country,
                        a.PhoneNumber_Value
                        FROM RescuedAnimalsMetadata ram
                        JOIN RescuedAnimals ra ON ram.Id = ra.Id
                        LEFT JOIN Adopters a ON ra.AdopterId_Value = a.Id";
            using var connection = new SqlConnection(configuration.GetConnectionString("Rescue"));
            var orderDetail = (await connection.QueryAsync(sql)).ToList();
            return Ok(orderDetail);
        }
    }
}