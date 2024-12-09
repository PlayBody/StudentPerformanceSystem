using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceSystem.Data;
using StudentPerformanceSystem.Models;
using StudentPerformanceSystem.Services;

namespace StudentPerformanceSystem.Services
{

    public class ScoreAnalysisService : IScoreAnalysisService
    {
        private readonly StudentPerformanceContext _context;

        public ScoreAnalysisService(StudentPerformanceContext context)
        {
            _context = context;
        }

        public async Task<double> GetAverageScoreByClassAsync(int classId, string subject, DateTime date, int grade)
        {
            var scores = await _context.Scores
                .Where(s => s.Student.ClassID == classId && s.Test.Subject == subject && s.Test.Date == date && s.Student.GradeLevel == grade)
                .ToListAsync();

            return scores.Average(s => s.ScoreValue);
        }

        public async Task<ComparisonResult> CompareClassScoresAsync(int classId, string subject, string period1, string period2)
        {
            var scoresPeriod1 = await GetScoresByPeriodAsync(classId, subject, period1);
            var scoresPeriod2 = await GetScoresByPeriodAsync(classId, subject, period2);

            return new ComparisonResult
            {
                Period1 = period1,
                AveragePeriod1 = scoresPeriod1.Average(s => s.ScoreValue),
                Period2 = period2,
                AveragePeriod2 = scoresPeriod2.Average(s => s.ScoreValue),
                Difference = scoresPeriod2.Average(s => s.ScoreValue) - scoresPeriod1.Average(s => s.ScoreValue)
            };
        }
        public async Task<ComparisonResult> CompareStudentScoresAsync(int studentId, string subject, string period1, string period2)
        {
            var scoresPeriod1 = await GetScoresByPeriodAsync(studentId, subject, period1);
            var scoresPeriod2 = await GetScoresByPeriodAsync(studentId, subject, period2);

            return new ComparisonResult
            {
                Period1 = period1,
                AveragePeriod1 = scoresPeriod1.Average(s => s.ScoreValue),
                Period2 = period2,
                AveragePeriod2 = scoresPeriod2.Average(s => s.ScoreValue),
                Difference = scoresPeriod2.Average(s => s.ScoreValue) - scoresPeriod1.Average(s => s.ScoreValue)
            };
        }

        private async Task<IEnumerable<Score>> GetScoresByPeriodAsync(int classOrStudentId, string subject, string period)
        {
            // This method should be implemented to retrieve scores based on the period.
            // The period might be defined by a date range or a specific term.
            // For simplicity, let's assume period is a term like "Fall" or "Spring" and map it to dates.
            // Implement logic to map period to date ranges here.
            // Example: if period == "Fall", map to dates from September to December.
            // This is a placeholder for actual implementation.

            return await _context.Scores
                .Where(s => (s.Student.ClassID == classOrStudentId || s.StudentID == classOrStudentId) &&
                            s.Test.Subject == subject &&
                            s.Test.Date >= StartDateForPeriod(period) &&
                            s.Test.Date <= EndDateForPeriod(period))
                .ToListAsync();
        }

        private DateTime StartDateForPeriod(string period)
        {
            // Implement logic to return the start date for the given period.
            // This is a placeholder function.
            return DateTime.Now; // Replace with actual logic
        }

        private DateTime EndDateForPeriod(string period)
        {
            // Implement logic to return the end date for the given period.
            // This is a placeholder function.
            return DateTime.Now; // Replace with actual logic
        }

        public async Task<IEnumerable<RollupResult>> GetSchoolRollupAsync(int schoolId, string subject)
        {
            var results = await _context.Scores
                .Where(s => s.Student.Class.Teacher.SchoolID == schoolId && s.Test.Subject == subject)
                .GroupBy(s => new { s.Test.Subject, s.Test.Date })
                .Select(g => new RollupResult
                {
                    Subject = g.Key.Subject,
                    Date = g.Key.Date,
                    AverageScore = g.Average(s => s.ScoreValue),
                    Level = "School"
                })
                .ToListAsync();

            return results;
        }

        public async Task<IEnumerable<RollupResult>> GetNetworkRollupAsync(string subject)
        {
            var results = await _context.Scores
                .Where(s => s.Test.Subject == subject)
                .GroupBy(s => new { s.Test.Subject, s.Test.Date })
                .Select(g => new RollupResult
                {
                    Subject = g.Key.Subject,
                    Date = g.Key.Date,
                    AverageScore = g.Average(s => s.ScoreValue),
                    Level = "Network"
                })
                .ToListAsync();

            return results;
        }
    }

}