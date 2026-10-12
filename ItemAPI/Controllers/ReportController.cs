using ItemDataLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly ISqlData _db;
        public ReportController(ISqlData db) => _db = db;

        [HttpGet("summary")]
        public ActionResult Summary() => Ok(_db.GetSummary());

        [HttpGet("lowstock")]
        public ActionResult LowStock() => Ok(_db.ListLowStock());
    }
}