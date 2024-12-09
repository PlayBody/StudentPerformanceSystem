using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPerformanceSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentPerformanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "FoundationStaff")]
    public class KippFoundationController : ControllerBase
    {
        private readonly IScoreAnalysisService _scoreAnalysisService;

        public KippFoundationController(IScoreAnalysisService scoreAnalysisService)
        {
            _scoreAnalysisService = scoreAnalysisService;
        }

        [HttpGet("network-rollup")]
        public async Task<IActionResult> GetNetworkRollup(string subject)
        {
            var rollupResults = await _scoreAnalysisService.GetNetworkRollupAsync(subject);
            return Ok(rollupResults);
        }
    }
}
