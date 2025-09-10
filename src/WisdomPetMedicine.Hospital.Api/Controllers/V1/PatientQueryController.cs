using Asp.Versioning;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WisdomPetMedicine.Hospital.Api.Controllers.V1;

[ApiVersion("1.0")]
[ApiController]
[Route("[controller]")]
public class PatientQueryController(IConfiguration configuration,
                                    ILogger<PatientQueryController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            string sql = @"SELECT pm.*, p.BloodType, p.Weight, p.Status, p.UpdatedOn
                            FROM PatientsMetadata pm 
                            JOIN Patients p
                            ON pm.Id = p.Id";
            using var connection = new SqlConnection(configuration.GetConnectionString("Hospital"));
            var orderDetail = (await connection.QueryAsync(sql)).ToList();
            return Ok(orderDetail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return StatusCode(500, "Try again later.");
        }
    }
}