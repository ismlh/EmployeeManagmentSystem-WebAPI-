﻿

namespace RepositoryPatternWithUOW.BL.Models
{
    public class ProjectEmployees
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
