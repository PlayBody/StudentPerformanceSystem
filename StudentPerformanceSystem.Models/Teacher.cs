namespace StudentPerformanceSystem.Models
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int SchoolID { get; set; }
        public ICollection<Class> Classes { get; }

        public Teacher()
        {
            Classes = new HashSet<Class>();
        }
    }
}
