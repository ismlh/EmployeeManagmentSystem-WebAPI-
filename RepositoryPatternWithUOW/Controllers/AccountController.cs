


using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerModel)
        {
            var IsExistUser = await _userManager.FindByNameAsync(registerModel.UserName);
            if (IsExistUser != null) return BadRequest("User Name Already Exist Try Another One");
            var user = new ApplicationUser()
            {
                FullName = registerModel.FullName,
                UserName = registerModel.UserName,
                PhoneNumber = registerModel.PhoneNumber,
                Email = registerModel.Email
            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                return Created();
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
                return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginModel)
        {
            var User = await _userManager.FindByNameAsync(loginModel.UserName);
            if (User != null)
            {
                var result = await _userManager.CheckPasswordAsync(User, loginModel.Password);
                if(result)
                {

                    #region Claims

                    // User Claims ==> In Body
                    var userCliams = new List<Claim>();
                    // add Guid To Change Token Every TIme He Login
                    userCliams.Add(new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()));
                    userCliams.Add(new Claim(ClaimTypes.NameIdentifier, User.Id));
                    userCliams.Add(new Claim(ClaimTypes.Name, User.UserName));

                    // get Roles To Add To This User
                    var userRoles = await _userManager.GetRolesAsync(User);
                    foreach (var role in userRoles)
                        userCliams.Add(new Claim(ClaimTypes.Role, role));
                    #endregion


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

                    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                    // design My Token
                    var myToken = new JwtSecurityToken
                        (
                            issuer: configuration["JWT:Issuer"],
                            audience: configuration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            claims: userCliams,
                            signingCredentials:signingCredentials
                        );
                    // Now We Need To return Token
                    return Ok
                         (new
                         {
                             token = new JwtSecurityTokenHandler().WriteToken(myToken),
                             expired = DateTime.Now.AddHours(1)
                         }
                         );
                }
            }
            return BadRequest("Invalid UserName Or Password ");
        }
    }
}
