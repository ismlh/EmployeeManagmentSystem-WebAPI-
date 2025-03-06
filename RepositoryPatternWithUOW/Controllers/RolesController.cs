



using System.Data;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager,
                                UserManager<ApplicationUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Roles = await _roleManager.Roles.ToListAsync();
            var result = Roles.Select(r => new { r.Id, r.Name }).ToList();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Role = await _roleManager.Roles.FirstOrDefaultAsync(r=>r.Id==id.ToString());
            if (Role == null) return NotFound();

            var result =  new { Role.Id, Role.Name };
            return Ok(result);
        }


        [HttpGet("GetByName{RoleName}")]
        public async Task<IActionResult> GetByName(string Name)
        {
            var Role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == Name);
            if (Role == null) return NotFound();

            var result = new { Role.Id, Role.Name };
            return Ok(result);
        }

        [HttpGet("GetUsersInRole{RoleName}")]
        public async Task<IActionResult> GetUsersInRole(string RoleName)
        {
            var Role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == RoleName);
            if (Role == null) return NotFound();
            var users = await _userManager.GetUsersInRoleAsync(Role.Name);
            var result = new
            {
                Role = Role.Name,
                Users = users.Select(u => new
                {
                    u.FullName,
                    u.UserName,
                    u.PhoneNumber,
                    u.Email
                }).ToList()
            };
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add(RoleDto roleModel)
        {
            if (string.IsNullOrEmpty(roleModel.Name))
                return BadRequest("Enter Role Name");
            var role = await _roleManager.Roles.
                FirstOrDefaultAsync
                (r => r.Name.ToLower() == roleModel.Name.ToLower());
            if (role != null) return BadRequest("Role Exist");
            var Newrole = new IdentityRole() { Id = new Guid().ToString(), Name = roleModel.Name,NormalizedName=roleModel.Name.ToUpper() };
            var result = await _roleManager.CreateAsync(Newrole);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Created();
        }
        [HttpPut("{Name}")]
        public async Task<IActionResult> Edit(RoleDto roleModel,string Name)
        {
            if (string.IsNullOrEmpty(roleModel.Name))
                return BadRequest("Enter Role Name");
            var role = await _roleManager.Roles.
                FirstOrDefaultAsync
                (r => r.Name.ToLower() == Name.ToLower());
            if (role == null) return NotFound("No Matched Data");

            role.Name = roleModel.Name;
            role.NormalizedName = roleModel.Name;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok("Updated");
        }
        [HttpDelete("{Name}")]
        public async Task<IActionResult> Delete(string Name)
        {
            if (string.IsNullOrEmpty(Name))
                return BadRequest("Enter Role Name");
            var  role = await _roleManager.Roles.
                FirstOrDefaultAsync
                (r => r.Name.ToLower() == Name.ToLower());
            if (role == null) return NotFound("No Matched Data");
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok("Deleted");

        }
    }
}
