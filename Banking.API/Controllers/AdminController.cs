using Banking.API.Projections;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ProjectionReplayService _replayService;

        public AdminController(ProjectionReplayService replayService)
        {
            _replayService = replayService;
        }

        [HttpPost("rebuild-projections")]
        public async Task<IActionResult> RebuildProjections(CancellationToken cancellationToken)
        {
            await _replayService.ReplayAsync(cancellationToken);

            return NoContent();
        }
    }
}
