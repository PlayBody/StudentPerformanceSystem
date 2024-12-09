namespace StudentPerformanceSystem.Models
{
    public class Score
    {
        public int ScoreID { get; set; }
        public int StudentID { get; set; }
        public int TestID { get; set; }
        public double ScoreValue { get; set; }

        public required Student Student { get; set; }
        public required Test Test { get; set; }
    }
}
