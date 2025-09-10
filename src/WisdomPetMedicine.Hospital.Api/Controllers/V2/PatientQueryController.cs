using Asp.Versioning;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace WisdomPetMedicine.Hospital.Api.Controllers.V2;

[ApiVersion("2.0")]
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return StatusCode(500, "Try again later.");
        }
    }
}