using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RehberApi.Model;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;

namespace RehberApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RehberYetkiliController : ControllerBase
    {
        private readonly string _connectionString = "Server =DESKTOP-FIKBBE9; initial catalog = DbRehber; integrated security = true";

        [HttpGet("ListRehberYetkili")]
        public async Task<IActionResult> ListRehberYetkili() //rehberyetkili tablosundaki tüm verileri listeleme
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var values = (await connection.QueryAsync<TblRehberYetkili>("SELECT * FROM TblRehberYetkili")).AsList();
                if (values.Count > 0)
                {
                    return Ok(new { status = true, message = "Veriler başarıyla alında.", data = values });
                }
                else
                {
                    return Ok(new { status = false, message = "Veri bulunamadı" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("ListRehberYetkiliAdi")]
        public async Task<IActionResult> ListRehberYetkiliAdi(string yetkiliAdi) //yetkili adına göre arama
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var query = "SELECT * FROM TblRehberYetkili WHERE YetkiliAdi LIKE @YetkiliAdiPattern";
                var values = await connection.QueryAsync<TblRehber>(query, new { YetkiliAdiPattern = "%" + yetkiliAdi + "%" });
                if (values.Any())
                {
                    return Ok(new { status = true, message = "Veri başarıyla alındı", data = values });
                }
                else
                {
                    return Ok(new { status = false, message = "Veri bulunamadı" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("AddRehberYetkili")]
        public async Task<IActionResult> AddRehberYetkili(TblRehberYetkili model)
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO TblRehberYetkili (RehberID,YetkiliAdı,GSM,Mail) VALUES (@RehberID,@YetkiliAdı,@GSM,@Mail)";
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
        [HttpPut("UpdateRehberYetkili")]
        public async Task<IActionResult> UpdateRehberYetkili(TblRehberYetkili model)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var query = "UPDATE TblRehberYetkili SET RehberID=@RehberID, YetkiliAdı=@YetkiliAdı, GSM=@GSM, Mail=@Mail";
                int affectedRows = await connection.ExecuteAsync(query, model);
                if (affectedRows > 0)
                {
                    return Ok(new { status = true, message = "Veri başarıyla güncellendi.", data = model });
                }
                else
                {
                    return Ok(new { status = false, message = "ID bulunamadı." });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("DeleteRehberYetkili")]
        public async Task<IActionResult> DeleteRehberYetkili(int ID) //ıd ye göre silme
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                int affectedRows = await connection.ExecuteAsync("DELETE FROM TblRehberYetkili Where RehberYetkiliID = @ID", new { ID });
                //sorgu başarıyla çalışırsa 1 değerini dönecektir aksi halde 0 dönecektir
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
