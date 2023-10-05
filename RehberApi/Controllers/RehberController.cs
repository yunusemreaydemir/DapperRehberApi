using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RehberApi.Model;
using System.Data.SqlClient;
using System.Reflection;

namespace RehberApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RehberController : ControllerBase
    {
        private readonly string _connectionString = "Server =DESKTOP-FIKBBE9; initial catalog = DbRehber; integrated security = true";

        [HttpGet("ListRehber")]
        public async Task<IActionResult> ListRehber()
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var values = (await connection.QueryAsync<TblRehber>("Select * From TblRehber")).AsList();

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
                // Hata durumunda ise hata mesajını döndürebiliriz.
                //return StatusCode(500, new { status = false, message = "Sunucu hatası: " + ex.Message });
                throw ex;
            }
        }

        [HttpGet("ListRehberFirma")]
        public async Task<IActionResult> ListRehberFirma(string firmaAdi)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var query = "SELECT * FROM TblRehber WHERE FirmaAdi LIKE @FirmaAdiPattern";
                var values = await connection.QueryAsync<TblRehber>(query, new { FirmaAdiPattern = "%" + firmaAdi + "%" });

                if (values.Any())
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


        [HttpPost("AddRehber")]
        public async Task<IActionResult> AddRehber(TblRehber model)
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO TblRehber (FirmaAdi, GSM, Adres) VALUES (@FirmaAdi, @GSM, @Adres)";
                    var addRows = await connection.ExecuteAsync(query, model);

                    if (addRows > 0)
                    {
                        return Ok(new { status = true, message = "Veriler başarıyla eklendi.", data = model });
                    }
                    else
                    {
                        return Ok(new { status = false, message = "Veri eklenemedi." });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateRehber")]
        public async Task<IActionResult> UpdateRehber(TblRehber model)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var query = "UPDATE TblRehber SET FirmaAdi=@FirmaAdi, GSM=@GSM, Adres=@Adres WHERE RehberID=@RehberID";
                int affectedRows = await connection.ExecuteAsync(query, model);

                if (affectedRows > 0)
                {
                    return Ok(new { status = true, message = "Veri Başarıyla Güncellendi." });
                }
                else
                {
                    return Ok(new { status = false, message = "ID Bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("DeleteRehber")]
        public async Task<IActionResult> DeleteRehber(int ID)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                int affectedRows = await connection.ExecuteAsync("DELETE FROM TblRehber Where RehberID = @ID", new { ID });
                if (affectedRows > 0)
                {
                    return Ok(new { status = true, message = "Veri başarıyla silindi." });
                }
                else
                {
                    return Ok(new { status = false, message = "ID bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = "Sunucu hatası: " + ex.Message });
            }
        }
    }
}
