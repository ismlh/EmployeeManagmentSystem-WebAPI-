


namespace RepositoryPatternWithUOW.BL.Repositories
{
    public interface IDepartmentRepository:IGenericRepository<Department>
    {
        Task<IEnumerable<Department>> GetDepartmentsWithEmployeesAndProjects();
    }
}
