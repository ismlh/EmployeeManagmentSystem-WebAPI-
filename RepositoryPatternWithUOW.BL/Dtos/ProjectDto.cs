

namespace RepositoryPatternWithUOW.BL.Dtos
{
    public class ProjectDto
    {
        public required string Name { get; set; }

        public string? Description { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
    }
}
