

namespace RepositoryPatternWithUOW.BL.Models
{
    public class Department
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public  string? Location { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
