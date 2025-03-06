

namespace RepositoryPatternWithUOW.DataAccessLayer.Services
{
    public class GenericReoistory<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        internal DbSet<T> _dbSet;
        public GenericReoistory(ApplicationDbContext context)
        {
            _context = context;
            _dbSet= _context.Set<T>();
        }
        public async Task Add(T entity)
        {
           _dbSet.Add(entity);
        }

        public async Task Delete(T entity)
        {
          
                _dbSet.Remove(entity);
            
        }

        public async Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
