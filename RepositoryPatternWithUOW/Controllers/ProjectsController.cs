

using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class ProjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectsController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Projects=await _unitOfWork.Projects.GetAll();
            var result = Projects.Select(p => new ProjectDto 
                { 
                    Name = p.Name, 
                    Description = p.Description ,
                    DepartmentId=p.DepartmentId
                }
            );
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _unitOfWork.Projects.GetById(id);
            if (project == null)
            {
                return NotFound();
            }
            else
            {
                var result = new ProjectDto
                {
                    Name = project.Name,
                    Description = project.Description,
                    DepartmentId = project.DepartmentId
                };
                return Ok(result);
            }
        }

        [HttpGet("GetByName")]

        public async Task<IActionResult> GetByName(string Name)
        {
            var projects = await _unitOfWork.Projects.Filter(d => d.Name == Name);
            if (!projects.Any())
            {
                return NotFound();
            }
            else
            {
                var result = projects.Select(d => new ProjectDto
                { Name = d.Name, Description = d.Description });
                return Ok(result);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(ProjectDto project)
        {
            var projectToAdd = new Project()
            {
                Name = project.Name,
                Description = project.Description,
                DepartmentId = project.DepartmentId
            };

            try
            {
                await _unitOfWork.Projects.Add(projectToAdd);
                _unitOfWork.Complete();
                return Ok(projectToAdd);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(ProjectDto projectModel,int id) 
        { 
            var project=await _unitOfWork.Projects.GetById(id);
            if(project == null)
            {
                return NotFound();
            }
            else
            {
                project.Name = projectModel.Name;
                project.Description = projectModel.Description;
                project.DepartmentId = projectModel.DepartmentId;
                try
                {
                    await _unitOfWork.Projects.Update(project);
                    _unitOfWork.Complete();
                    return Ok(project);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _unitOfWork.Projects.GetById(id);
            if (project == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    await _unitOfWork.Projects.Delete(project);
                    _unitOfWork.Complete();
                    return Ok(project);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
