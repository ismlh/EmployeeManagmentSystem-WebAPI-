

namespace RepositoryPatternWithUOW.DataAccessLayer.Services
{
    public class ProjectEmployeeService:GenericReoistory<ProjectEmployees>, IProjectEmployees
    {
        public ProjectEmployeeService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
