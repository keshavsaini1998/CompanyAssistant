using CompanyAssistant.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyAssistant.Api.Controllers
{
    [Route("api/ask")]
    [ApiController]
    public class AskController : ControllerBase
    {
        private readonly AskQuestionHandler _handler;
        public AskController(AskQuestionHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Ask(AskQuestionCommand cmd)
        {
            var result = await _handler.Handle(cmd);
            return Ok(result);
        }
    }
}
