
using ItemDataLibrary;
using ItemDataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ItemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ISqlData _db;
        public TransactionController(ISqlData db) => _db = db;

        [HttpGet]
        public ActionResult Get([FromQuery] DateTime? from, [FromQuery] DateTime? to)
            => Ok(_db.ListTransactions(from, to));

        [HttpPost]
        public ActionResult Post([FromBody] TransactionModel t)
        {
            if (t.Quantity <= 0) return BadRequest("Quantity must be greater than zero.");
            if (t.Type is not ("SALE" or "STOCKIN" or "ADJUST"))
                return BadRequest("Invalid transaction type.");

            t.CreatedBy = User.Identity?.Name ?? "unknown";
            try
            {
                _db.CreateTransaction(t);
                return Ok("Transaction recorded.");
            }
            catch (Exception ex) when (
                (ex as SqlException ?? ex.InnerException as SqlException) is SqlException sql
                && (sql.Number is 50001 or 50002 || sql.Message.Contains("Insufficient stock")))
            {
                var sqlEx = ex as SqlException ?? (SqlException)ex.InnerException!;
                return Conflict(sqlEx.Message);
            }
        }
    }
}