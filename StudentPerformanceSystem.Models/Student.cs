namespace StudentPerformanceSystem.Models
{
    public class Student
    {
        public int StudentID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int GradeLevel { get; set; }
        public int ClassID { get; set; }

        public required Class Class { get; set; }
    }
}
