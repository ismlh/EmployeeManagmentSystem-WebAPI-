



namespace RepositoryPatternWithUOW.DataAccessLayer.Services
{
    public class ProjectService : GenericReoistory<Project>, IProjectRepository
    {
        public ProjectService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
