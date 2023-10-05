using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RehberApi.Model;
using System.Data.SqlClient;
using System.Diagnostics;

namespace RehberApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RehberEtiketController : ControllerBase
    {
        private readonly string _connectionString = "Server =DESKTOP-FIKBBE9; initial catalog = DbRehber; integrated security = true";

        [HttpGet]
        public async Task<IActionResult> ListRehberEtiket()
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var values = (await connection.QueryAsync<TblRehberEtiket>("SELECT * FROM TblRehberEtiket")).AsList();
                if (values.Count > 0)
                {
                    return Ok(new { status = true, message = "Veriler başarıyla alındı.", data = values });
                }
                else
                {
                    return Ok(new { status = false, message = "Veri bulunamadı." });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ListRehberEtiketDetay")]
        public async Task<IActionResult> ListRehberEtiketDetay()
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var values = (await connection.QueryAsync<TblRehberEtiketDetay>("SELECT ret.RehberEtiketID, re.FirmaAdi, et.EtiketAdi " +
                    "FROM [DbRehber].[dbo].[TblRehberEtiket] ret " +
                    "INNER JOIN [DbRehber].[dbo].[TblRehber] re ON ret.RehberID = re.RehberID " +
                    "INNER JOIN [DbRehber].[dbo].[TblEtiket] et ON ret.EtiketID = et.EtiketID")).AsList();
                if (values.Count > 0)
                {
                    return Ok(new { status = true, message = "Veriler başarıyla alındı.", data = values });
                }
                else
                {
                    return Ok(new { status = false, message = "Veri bulunamadı." });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("AddRehberEtiket")]
        public async Task<IActionResult> AddRehberEtiket(TblRehberEtiket model)
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO TblRehberEtiket (RehberID,EtiketID) VALUES (@RehberID,@EtiketID)";
                    var addRows = await connection.ExecuteAsync(query, model);
                    if (addRows > 0)
                    {
                        return Ok(new { status = true, message = "Veriler başarıyla eklendi", data = model });
                    }
                    else
                    {
                        return Ok(new { status = false, message = "Veri eklenemedi" });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateRehberEtiket")]
        public async Task<IActionResult> UpdateRehberEtiket(TblRehberEtiket model)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var query = "UPDATE TblRehberEtiket SET RehberID = @RehberID, EtiketID=@EtiketID WHERE RehberEtiketID = @RehberEtiketID";
                int affectedRows = await connection.ExecuteAsync(query, model);
                if (affectedRows > 0)
                {
                    return Ok(new { status = true, message = "Veriler başarıyla güncellendi." });
                }
                else
                {
                    return Ok(new { status = false, message = "ID bulunamadı" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpDelete("DeleteRehberEtiket")]
        public async Task<IActionResult> DeleteRehberEtiket(int ID)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                int affectedRows = await connection.ExecuteAsync("DELETE FROM TblRehberEtiket Where RehberEtiketID = @ID", new { ID });
                if (affectedRows > 0)
                {
                    return Ok(new { status = true, message = "Veri başarıyla silindi" });
                }
                else
                {
                    return Ok(new { status = false, message = "ID bulunamadı" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
