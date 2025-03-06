


namespace RepositoryPatternWithUOW.DataAccessLayer.Services
{
    public class DepartmentService : GenericReoistory<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentService(ApplicationDbContext context) : base(context)
        {

            _context = context;
        }

        public async Task<IEnumerable<Department>> GetDepartmentsWithEmployeesAndProjects()
        {
            return await _context.Departments.
                Include(d => d.Employees).
                ThenInclude(d => d.Projects).ToListAsync();
        }
    }
}
