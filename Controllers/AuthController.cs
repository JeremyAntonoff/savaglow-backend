using System.Threading.Tasks;
using AutoMapper;
using Savaglow.Dtos;
using Savaglow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NomoBucket.API.Dtos;
using Savaglow.Helpers;
using Microsoft.Extensions.Configuration;

namespace Savaglow.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(IMapper mapper, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            var userToCreate = _mapper.Map<User>(userRegistrationDto);
            var createdUser = await _userManager.CreateAsync(userToCreate, userRegistrationDto.Password);
            if (createdUser.Succeeded)
            {
                return Ok();
            }
            return BadRequest(createdUser.Errors);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.Username);
            if (user != null)
            {
                var loginAttempt = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);

                if (loginAttempt.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserDetailsDto>(user);
                    var authHelper = new AuthHelper(_configuration);
                    var token = authHelper.GenerateToken(user);
                    return Ok(new
                    {
                        token,
                        user = userToReturn
                    });
                }
            }

            return Unauthorized();
        }
    }
}