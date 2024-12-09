using StudentPerformanceSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentPerformanceSystem.Services
{
    public interface ITestScoreService
    {
        Task<IEnumerable<Score>> GetAllScoresAsync();
        Task<Score?> GetScoreByIdAsync(int studentId, int testId);
        Task AddScoreAsync(Score score);
        Task UpdateScoreAsync(Score score);
        Task DeleteScoreAsync(int studentId, int testId);
        Task<IEnumerable<Score>> GetScoresByStudentIdAsync(int studentId);
    }
}
