

using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]

    public class RelativesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public RelativesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Relatives = await _unitOfWork.Relatives.GetRelativesWithEmployees();
            var result = Relatives.
                Select(r => new
                {
                    r.Name,
                    r.Job,
                    r.Age,
                    Employee = r.Employee.FName + " " + r.Employee.LName
        }
                );
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var relative = await _unitOfWork.Relatives.GetById(id);
            if (relative == null) return NotFound("No Matched Data");
            var result = await _unitOfWork.Relatives.GetRelativeWithEmployee(id);
            return Ok(new { result.Name, result.Job, result.Age, Employee = result.Employee.FName + " " + result.Employee.LName });
        }
        [HttpPost]
        public async Task<IActionResult> Add(RelativeDto relativeModel)
        {
            var relative = new Relative
            {
                Name = relativeModel.Name,
                Age = relativeModel.Age,
                Job=relativeModel.Job,
                EmolyeeId=relativeModel.EmolyeeId
            };
            try
            {
                await _unitOfWork.Relatives.Add(relative);
                _unitOfWork.Complete();
                return Created();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> edit(RelativeDto relativeModel ,int id)
        {
            var relative = await _unitOfWork.Relatives.GetById(id);
            if (relative == null) return NotFound("No Matched Data");
            try
            {
                relative.Name = relativeModel.Name;
                relative.Age = relativeModel.Age;
                relative.Job = relativeModel.Job;
                relative.EmolyeeId = relativeModel.EmolyeeId;
                await _unitOfWork.Relatives.Update(relative);
                _unitOfWork.Complete();
                return Ok("Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var relative = await _unitOfWork.Relatives.GetById(id);
            if (relative == null) return NotFound("No Matched Data");
            try
            {
                
                await _unitOfWork.Relatives.Delete(relative);
                _unitOfWork.Complete();
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
