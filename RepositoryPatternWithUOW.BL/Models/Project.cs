


namespace RepositoryPatternWithUOW.BL.Models
{
    public class Project
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        
        public Department Department { get; set; }


        //[ForeignKey("Employee")]
        //public int LeaderID { get; set; }

        //public Employee Employee { get; set; }


        public ICollection<Employee> Employees { get; set; }
        public ICollection<ProjectEmployees> ProjectEmployees { get; set; }


    }
}
