using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using StudentPerformanceSystem.Data;
using StudentPerformanceSystem.Models;
using StudentPerformanceSystem.Services;

namespace StudentPerformanceSystem.Tests
{
    public class ScoreAnalysisServiceTests
    {
        private readonly Mock<DbSet<Score>> _mockScoreSet;
        private readonly Mock<StudentPerformanceContext> _mockContext;
        private readonly ScoreAnalysisService _service;

        public ScoreAnalysisServiceTests()
        {
            _mockScoreSet = new Mock<DbSet<Score>>();
            _mockContext = new Mock<StudentPerformanceContext>(new DbContextOptions<StudentPerformanceContext>());
            _mockContext.Setup(m => m.Scores).Returns(_mockScoreSet.Object);

            _service = new ScoreAnalysisService(_mockContext.Object);
        }

        [Fact]
        public async Task GetAverageScoreByClassAsync_ReturnsCorrectAverage()
        {
            // Arrange
            var scores = new List<Score>
            {
                new Score { StudentID = 1, TestID = 1, ScoreValue = 85, Student = new Student { ClassID = 1, GradeLevel = 10, FirstName = "A", LastName = "B", Class = new Class { ClassName = "XX", Teacher = new Teacher { FirstName = "F", LastName = "C" } }}, Test = new Test { Subject = "Math", Date = DateTime.Today } },
                new Score { StudentID = 2, TestID = 1, ScoreValue = 95, Student = new Student { ClassID = 1, GradeLevel = 10, FirstName = "A", LastName = "B", Class = new Class { ClassName = "XX", Teacher = new Teacher { FirstName = "F", LastName = "C" } }}, Test = new Test { Subject = "Math", Date = DateTime.Today } }
            }.AsQueryable();

            _mockScoreSet.As<IQueryable<Score>>().Setup(m => m.Provider).Returns(scores.Provider);
            _mockScoreSet.As<IQueryable<Score>>().Setup(m => m.Expression).Returns(scores.Expression);
            _mockScoreSet.As<IQueryable<Score>>().Setup(m => m.ElementType).Returns(scores.ElementType);
            _mockScoreSet.As<IQueryable<Score>>().Setup(m => m.GetEnumerator()).Returns(scores.GetEnumerator());

            // Act
            var average = await _service.GetAverageScoreByClassAsync(1, "Math", DateTime.Today, 10);

            // Assert
            Assert.Equal(90, average);
        }

        [Fact]
        public async Task CompareClassScoresAsync_ReturnsCorrectComparisonResult()
        {
            // Arrange
            var scoresPeriod1 = new List<Score>
            {
                new Score { StudentID = 1, TestID = 1, ScoreValue = 75, Student = new Student { ClassID = 1, FirstName = "A", LastName = "B", Class = new Class { ClassName = "XX", Teacher = new Teacher { FirstName = "F", LastName = "C" } } }, Test = new Test { Subject = "Math", Date = new DateTime(2022, 9, 1) } },
                new Score { StudentID = 2, TestID = 1, ScoreValue = 85, Student = new Student { ClassID = 1, FirstName = "A", LastName = "B", Class = new Class { ClassName = "XX", Teacher = new Teacher { FirstName = "F", LastName = "C" } } }, Test = new Test { Subject = "Math", Date = new DateTime(2022, 9, 1) } },
            }.AsQueryable();

            var scoresPeriod2 = new List<Score>
            {
                new Score { StudentID = 1, TestID = 2, ScoreValue = 80, Student = new Student { ClassID = 1, FirstName = "A", LastName = "B", Class = new Class { ClassName = "XX", Teacher = new Teacher { FirstName = "F", LastName = "C" } }}, Test = new Test { Subject = "Math", Date = new DateTime(2023, 3, 1) } },
                new Score { StudentID = 2, TestID = 2, ScoreValue = 90, Student = new Student { ClassID = 1, FirstName = "A", LastName = "B", Class = new Class { ClassName = "XX", Teacher = new Teacher { FirstName = "F", LastName = "C" } }}, Test = new Test { Subject = "Math", Date = new DateTime(2023, 3, 1) } }
            }.AsQueryable();

            _mockScoreSet.As<IQueryable<Score>>().Setup(m => m.Provider).Returns(scoresPeriod1.Concat(scoresPeriod2).Provider);
            _mockScoreSet.As<IQueryable<Score>>().Setup(m => m.Expression).Returns(scoresPeriod1.Concat(scoresPeriod2).Expression);
            _mockScoreSet.As<IQueryable<Score>>().Setup(m => m.ElementType).Returns(scoresPeriod1.Concat(scoresPeriod2).ElementType);
            _mockScoreSet.As<IQueryable<Score>>().Setup(m => m.GetEnumerator()).Returns(scoresPeriod1.Concat(scoresPeriod2).GetEnumerator());

            // Act
            var comparisonResult = await _service.CompareClassScoresAsync(1, "Math", "Fall", "Spring");

            // Assert
            Assert.Equal("Fall", comparisonResult.Period1);
            Assert.Equal(80, comparisonResult.AveragePeriod1);
            Assert.Equal("Spring", comparisonResult.Period2);
            Assert.Equal(85, comparisonResult.AveragePeriod2);
            Assert.Equal(5, comparisonResult.Difference);
        }
    }
}
