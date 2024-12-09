using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentPerformanceSystem.Models;

namespace StudentPerformanceSystem.Services
{
    public interface IScoreAnalysisService
    {
        Task<double> GetAverageScoreByClassAsync(int classId, string subject, DateTime date, int grade);
        Task<ComparisonResult> CompareClassScoresAsync(int classId, string subject, string period1, string period2);
        Task<ComparisonResult> CompareStudentScoresAsync(int studentId, string subject, string period1, string period2);
        Task<IEnumerable<RollupResult>> GetSchoolRollupAsync(int schoolId, string subject);
        Task<IEnumerable<RollupResult>> GetNetworkRollupAsync(string subject);
    }

    public class ComparisonResult
    {
        public string? Period1 { get; set; }
        public double AveragePeriod1 { get; set; }
        public string? Period2 { get; set; }
        public double AveragePeriod2 { get; set; }
        public double Difference { get; set; }
    }

    public class RollupResult
    {
        public string? Subject { get; set; }
        public DateTime Date { get; set; }
        public double AverageScore { get; set; }
        public string? Level { get; set; } // e.g., "Teacher", "Grade", "School"
    }
}
