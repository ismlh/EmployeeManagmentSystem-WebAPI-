


namespace RepositoryPatternWithUOW.DataAccessLayer.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEmployeeRepository Employees { get; private set; }

        public IDepartmentRepository Departments { get; private set; }

        public IProjectRepository Projects { get; private set; }
        public IProjectEmployees ProjectEmployees { get; private set; }
        public IRelativeRepository Relatives { get; private set; }

        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Employees = new EmployeeService(_context);
            Departments = new DepartmentService(_context);
            Projects = new ProjectService(_context);
            ProjectEmployees = new ProjectEmployeeService(_context);
            Relatives = new RelativeService(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
