using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Hospital.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("[controller]")]
    public class PatientQueryController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public PatientQueryController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string sql = @"SELECT pm.Id,
                            pm.Name,
                            pm.Breed,
                            Sex = 
                            CASE pm.Sex
	                            WHEN 0 THEN 'Male'
	                            WHEN 1 THEN 'Female'
                            END,
                            pm.Color,
                            pm.DateOfBirth,
                            pm.Species,
                            p.BloodType, 
                            p.Weight, 
                            p.Status, 
                            p.UpdatedOn
                            FROM PatientsMetadata pm 
                            JOIN Patients p
                            ON pm.Id = p.Id";
            using var connection = new SqlConnection(configuration.GetConnectionString("Hospital"));
            var orderDetail = (await connection.QueryAsync(sql)).ToList();
            return Ok(orderDetail);
        }
    }
}