

namespace RepositoryPatternWithUOW.BL.Repositories
{
   public interface IRelativeRepository:IGenericRepository<Relative>
    {
       Task<IEnumerable< Relative>> GetRelativesWithEmployees();
       Task<Relative> GetRelativeWithEmployee(int id);
    }
}
