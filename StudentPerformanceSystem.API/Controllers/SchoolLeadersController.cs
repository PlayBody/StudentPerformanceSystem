using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPerformanceSystem.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentPerformanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SchoolLeader")]
    public class SchoolLeadersController : ControllerBase
    {
        private readonly IScoreAnalysisService _scoreAnalysisService;

        public SchoolLeadersController(IScoreAnalysisService scoreAnalysisService)
        {
            _scoreAnalysisService = scoreAnalysisService;
        }

        [HttpGet("school-rollup")]
        public async Task<IActionResult> GetSchoolRollup(int schoolId, string subject)
        {
            var rollupResults = await _scoreAnalysisService.GetSchoolRollupAsync(schoolId, subject);
            return Ok(rollupResults);
        }
    }
}
