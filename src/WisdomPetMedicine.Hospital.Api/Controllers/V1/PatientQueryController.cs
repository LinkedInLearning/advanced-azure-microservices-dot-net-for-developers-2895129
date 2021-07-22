using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Hospital.Api.Controllers.V1
{
    [ApiVersion("1.0")]
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
            string sql = @"SELECT pm.*, p.BloodType, p.Weight, p.Status, p.UpdatedOn
                            FROM PatientsMetadata pm 
                            JOIN Patients p
                            ON pm.Id = p.Id";
            using var connection = new SqlConnection(configuration.GetConnectionString("Hospital"));
            var orderDetail = (await connection.QueryAsync(sql)).ToList();
            return Ok(orderDetail);
        }
    }
}