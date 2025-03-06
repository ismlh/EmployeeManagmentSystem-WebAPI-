




namespace RepositoryPatternWithUOW.DataAccessLayer.Services
{
    public class RelativeService : GenericReoistory<Relative>, IRelativeRepository
    {
        private readonly ApplicationDbContext _context;
        public RelativeService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Relative>> GetRelativesWithEmployees()
        {
            return await _context.Relatives.Include(r => r.Employee).ToListAsync();

        }

        public async Task<Relative> GetRelativeWithEmployee(int id)
        {
            return await _context.Relatives.Include(r => r.Employee).FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
