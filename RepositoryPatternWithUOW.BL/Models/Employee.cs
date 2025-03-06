using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryPatternWithUOW.BL.Models
{
   public class Employee
    {
        public int Id { get; set; }
        public required string FName { get; set; }
        public required string LName { get; set; }
        public  double? Salary { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<ProjectEmployees> ProjectEmployees { get; set; }


        // Self-referencing relationship: Manager
        public int? ManagerId { get; set; } // Nullable for employees without a manager
        public Employee Manager { get; set; }

        // Self-referencing relationship: Subordinates
        public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();


        public virtual ICollection<Relative> Relatives { get; set; }

    }
}
