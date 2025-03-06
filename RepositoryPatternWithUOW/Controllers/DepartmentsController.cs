



namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _unitOfWork.Departments.GetAll();
            var result=departments.Select(d => new DepartmentDto { Location = d.Location, Name = d.Name });
            return Ok(result);
        }

        [HttpGet("GetAllWithEmployeesAndProjects")]
        public async Task<IActionResult> GetAllWithEmployeesAndProjects()
        {
            var departments = await _unitOfWork.Departments.GetDepartmentsWithEmployeesAndProjects();
            var result = departments.Select(d => new 
            { 
                Location = d.Location,
                Name = d.Name ,
                EmployeesCount = d.Employees.Count,
                Employees=d.Employees.Select(e=>new {e.FName,e.LName}).ToArray(),
                ProjectsCount = d.Projects.Count,
                Projects = d.Projects.Select(e => new { e.Name, e.Description }).ToArray()
            });
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var department = await _unitOfWork.Departments.GetById(id);
            if(department == null)
            {
                return NotFound();
            }
            else
            {
                var result= new DepartmentDto { Name = department.Name,Location=department.Location };
                return Ok(result);
            }
        }
        [HttpGet("GetByName")]

        public async Task<IActionResult> GetByName(string Name)
        {
            var departments = await _unitOfWork.Departments.Filter(d=>d.Name==Name);
            if (!departments.Any())
            {
                return NotFound();
            }
            else
            {
                var result = departments.Select(d=> new DepartmentDto { Name = d.Name, Location = d.Location });
                return Ok(result);
            }
        }
        [HttpPost]

        public async Task<IActionResult> Add(DepartmentDto departmentModel)
        {
            if(string.IsNullOrEmpty(departmentModel.Name)) 
                return BadRequest("Name Is Required");
            var department = new Department() { Name=departmentModel.Name,Location=departmentModel.Location };
            try
            {
                await _unitOfWork.Departments.Add(department);
                _unitOfWork.Complete();
                return Ok(department);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(DepartmentDto departmentModel,int id)
        {
            if (string.IsNullOrEmpty(departmentModel.Name))
                return BadRequest("Name Is Required");
            var department = await _unitOfWork.Departments.GetById(id);
            if(department == null)
            {
                return NotFound();
            }

            try
            {
                department.Name = departmentModel.Name;
                if(!string.IsNullOrEmpty(departmentModel.Location))
                {
                    department.Location = departmentModel.Location;
                }
                await _unitOfWork.Departments.Update(department);
                _unitOfWork.Complete();
                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deelete( int id)
        {
            var department = await _unitOfWork.Departments.GetById(id);
            if (department == null)
            {
                return NotFound();
            }
            try
            {
                await _unitOfWork.Departments.Delete(department);
                _unitOfWork.Complete();
                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
