

namespace RepositoryPatternWithUOW.BL.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IEmployeeRepository Employees { get; }
        IDepartmentRepository Departments { get; }
        IProjectRepository Projects { get; }
        IProjectEmployees ProjectEmployees { get; }
        IRelativeRepository Relatives { get; }

        public int Complete();
    }
}
