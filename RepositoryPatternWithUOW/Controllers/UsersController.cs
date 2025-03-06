

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<Object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new
                {
                    user.FullName,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    Roles = roles
                });
            }

            return Ok(result);
        }

        [HttpGet("GetByUserName{UserName}")]
        public async Task<IActionResult> GetByUserName(string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
                return BadRequest("User Name Required");
            var user = await _userManager.FindByNameAsync(UserName);
            if(user==null)
                return BadRequest("No Users With This UserName");
          
            return Ok(user);
        }

        [HttpGet("GetByEmail{Email}")]
        public async Task<IActionResult> GetByEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return BadRequest("Email Required");
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return BadRequest("No Users With This Email");
            return Ok(user);
        }

        [HttpGet("GetPyPhoneNumber")]
        public async Task<IActionResult> GetPyPhoneNumber(string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return BadRequest("PhoneNumber Required");
            var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.PhoneNumber== PhoneNumber);
            if (user == null)
                return BadRequest("No Users With This Phone Number");
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UserDto userDto)
        {
            var existingUser = await _userManager.FindByNameAsync(userDto.UserName);
            if (existingUser != null)
            {
                return BadRequest("User Name Already Exists. Try Another One.");
            }

            var user = new ApplicationUser
            {
                FullName = userDto.FullName,
                UserName = userDto.UserName,
                PhoneNumber = userDto.PhoneNumber,
                Email = userDto.Email
            };

            var createUserResult = await _userManager.CreateAsync(user, userDto.Password);
            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            if (userDto.Roles?.Any() == true)
            {
                foreach (var role in userDto.Roles)
                {
                    if (!string.IsNullOrWhiteSpace(role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
            }

            return Created();
        }

        [HttpPut("{UserName}")]
        public async Task<IActionResult> Edit(UserEditDto userDto, string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
                return BadRequest("Enter UserName");

            var existingUser = await _userManager.FindByNameAsync(UserName);
            if (existingUser == null)
            {
                return NotFound("No Matched User Name");
            }

            // Update non-password fields
            existingUser.FullName = userDto.FullName;
            existingUser.Email = userDto.Email;
            existingUser.PhoneNumber = userDto.PhoneNumber;

            // Update the user's details
            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            // If a new password is provided, update the password
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                // Generate a password reset token
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

                // Use ResetPasswordAsync to update the password
                var resetPasswordResult = await _userManager.ResetPasswordAsync(existingUser, token, userDto.Password);
                if (!resetPasswordResult.Succeeded)
                {
                    foreach (var error in resetPasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }

            return Ok("Updated");
        }
        [HttpDelete("UserName")]
        public async Task<IActionResult> Delete(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
                return BadRequest("No User With This User Name");
            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // If deletion fails, return errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return BadRequest(ModelState);
            }

            // Return success message
            return Ok("User Deleted Successfully");
        }
    }
}
