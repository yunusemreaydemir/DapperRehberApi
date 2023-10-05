using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using RehberApi.Model;
using System.Data.SqlClient;

namespace RehberApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EtiketController : ControllerBase
    {
        private readonly string _connectionString = "Server =DESKTOP-FIKBBE9; initial catalog = DbRehber; integrated security = true";

        [HttpGet("ListEtiket")]
        public async Task<IActionResult> ListEtiket()
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var values = (await connection.QueryAsync<TblEtiket>("SELECT * FROM TblEtiket")).AsList();
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

        [HttpGet("ListEtiketAdı")]
        public async Task<IActionResult> ListEtiketAdı(string etiketAdi)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var query = "SELECT * FROM TblEtiket WHERE EtiketAdi LIKE @EtiketAdiPattern";
                var values = await connection.QueryAsync<TblEtiket>(query, new { EtiketAdiPattern = "%" + etiketAdi + "%" });

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

        [HttpPost("AddEtiket")]
        public async Task<IActionResult> AddEtiket(TblEtiket model)
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "INSERT INTO Tbletiket (EtiketAdi) VALUES (@EtiketAdi)";
                    var addRows = await (connection.ExecuteAsync(query, model));
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

        [HttpPut("UpdateEtiket")]
        public async Task<IActionResult> UpdateEtiket(TblEtiket model)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                var query = "UPDATE TblEtiket SET EtiketAdi = @EtiketAdi WHERE EtiketID=@EtiketID";
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
        [HttpDelete("DeleteEtiket")]
        public async Task<IActionResult> DeleteEtiket(int ID)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                int affectedRows = await connection.ExecuteAsync("DELETE FROM TblEtiket WHERE EtiketID = @ID", new { ID });
                if (affectedRows > 0)
                {
                    return Ok(new { status = true, message = "Veri Başarıyla Silindi." });
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
    }
}
