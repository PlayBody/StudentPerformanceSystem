namespace StudentPerformanceSystem.Models
{
    public class Class
    {
        public int ClassID { get; set; }
        public int TeacherID { get; set; }
        public required string ClassName { get; set; }
        public ICollection<Student> Students { get; }
        public required Teacher Teacher { get; set; }

        public Class()
        {
            Students = new HashSet<Student>();
        }
    }
}
