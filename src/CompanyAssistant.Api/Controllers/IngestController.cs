using CompanyAssistant.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAssistant.Api.Controllers
{
    //[Authorize]
    [Route("api/ingest")]
    [ApiController]
    public class IngestController : ControllerBase
    {
        private readonly ISqlReader _sqlReader;
        private readonly IVectorStore _vector;

        public IngestController(
            ISqlReader sqlReader,
            IVectorStore vector)
        {
            _sqlReader = sqlReader;
            _vector = vector;
        }

        [HttpPost]
        public async Task<IActionResult> Ingest(Guid ProjectId)
        {
            var docs = await _sqlReader.ReadAsync(ProjectId);
            await _vector.StoreAsync(docs);
            return Ok("Ingested");
        }
    }
}
