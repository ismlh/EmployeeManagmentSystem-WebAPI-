using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternWithUOW.BL.Dtos
{
    public class EmployeeDto
    {
        public required string FName { get; set; }
        public required string LName { get; set; }
        public double? Salary { get; set; }

        public int? DepartmentId { get; set; }
        public int? ProjectId { get; set; }

        public int? ManagerId { get; set; } // Nullable for employees without a manager

    }

}
