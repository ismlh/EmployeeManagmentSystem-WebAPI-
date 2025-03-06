

using System.Linq.Expressions;

namespace RepositoryPatternWithUOW.BL.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task <IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate);
    }
}
