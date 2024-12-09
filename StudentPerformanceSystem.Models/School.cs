namespace StudentPerformanceSystem.Models
{
    public class School
    {
        public int SchoolID { get; set; }
        public required string SchoolName { get; set; }
        public string? Location { get; set; }

        public ICollection<Teacher> Teachers { get; set; }

        public School()
        {
            Teachers = new HashSet<Teacher>();
        }
    }
}
