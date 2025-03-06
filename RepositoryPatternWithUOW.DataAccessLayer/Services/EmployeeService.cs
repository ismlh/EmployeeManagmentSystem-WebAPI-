

namespace RepositoryPatternWithUOW.DataAccessLayer.Services
{
    public class EmployeeService : GenericReoistory<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesWithRelatvies()
        {
            return await _context.Employees.Include(e => e.Relatives).ToListAsync();
        }
    }
}
