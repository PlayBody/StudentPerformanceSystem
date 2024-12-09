using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPerformanceSystem.Services;
using System.Threading.Tasks;

namespace StudentPerformanceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeachersController : ControllerBase
    {
        private readonly IScoreAnalysisService _scoreAnalysisService;

        public TeachersController(IScoreAnalysisService scoreAnalysisService)
        {
            _scoreAnalysisService = scoreAnalysisService;
        }

        [HttpGet("average-score")]
        public async Task<IActionResult> GetAverageScore(int classId, string subject, DateTime date, int grade)
        {
            var averageScore = await _scoreAnalysisService.GetAverageScoreByClassAsync(classId, subject, date, grade);
            return Ok(averageScore);
        }

        [HttpGet("compare-class-scores")]
        public async Task<IActionResult> CompareClassScores(int classId, string subject, string period1, string period2)
        {
            var comparisonResult = await _scoreAnalysisService.CompareClassScoresAsync(classId, subject, period1, period2);
            return Ok(comparisonResult);
        }

        [HttpGet("compare-student-scores")]
        public async Task<IActionResult> CompareStudentScores(int studentId, string subject, string period1, string period2)
        {
            var comparisonResult = await _scoreAnalysisService.CompareStudentScoresAsync(studentId, subject, period1, period2);
            return Ok(comparisonResult);
        }
    }
}
