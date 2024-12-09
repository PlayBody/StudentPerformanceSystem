namespace StudentPerformanceSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; } // "Teacher", "SchoolLeader", "FoundationStaff"
        public int? TeacherID { get; set; }
        public int? SchoolID { get; set; }
    }
}
