using Microsoft.EntityFrameworkCore;
using StudentPerformanceSystem.Data;
using StudentPerformanceSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentPerformanceSystem.Services
{
    public class TestScoreService : ITestScoreService
    {
        private readonly StudentPerformanceContext _context;

        public TestScoreService(StudentPerformanceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Score>> GetAllScoresAsync()
        {
            return await _context.Scores.ToListAsync();
        }

        public async Task<Score?> GetScoreByIdAsync(int studentId, int testId)
        {
            return await _context.Scores.FindAsync(studentId, testId);
        }

        public async Task AddScoreAsync(Score score)
        {
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateScoreAsync(Score score)
        {
            _context.Entry(score).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteScoreAsync(int studentId, int testId)
        {
            var score = await _context.Scores.FindAsync(studentId, testId);
            if (score != null)
            {
                _context.Scores.Remove(score);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Score>> GetScoresByStudentIdAsync(int studentId)
        {
            return await _context.Scores
                .Where(s => s.StudentID == studentId)
                .ToListAsync();
        }
    }
}
