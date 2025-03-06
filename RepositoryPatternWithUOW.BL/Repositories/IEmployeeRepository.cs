

using RepositoryPatternWithUOW.BL.Models;

namespace RepositoryPatternWithUOW.BL.Repositories
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetEmployeesWithRelatvies();
    }
    
}
