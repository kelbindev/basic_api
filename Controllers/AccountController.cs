using empty_template.Services;
using empty_template.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using empty_template.DTO.Account;
using empty_template.Models.Account;

namespace empty_template.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly TokenService tokenService;

        public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        TokenService tokenService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            Console.WriteLine(loginDTO);
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDTO.Email);

            if (user == null) { return Unauthorized(); }

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (result.Succeeded)
            {
                return createUserObject(user);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromForm]RegisterViewModel RegisterViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await userManager.Users.AnyAsync(x => x.Email == RegisterViewModel.Email))
                {
                    ModelState.AddModelError("email", "Email Taken");
                }

                if (await userManager.Users.AnyAsync(x => x.UserName == RegisterViewModel.UserName))
                {
                    ModelState.AddModelError("username", "Username Taken");
                }

                if (ModelState.ErrorCount > 0) return ValidationProblem();

                var user = new AppUser
                {
                    UserName = RegisterViewModel.UserName,
                    Email = RegisterViewModel.Email,
                    DisplayName = RegisterViewModel.DisplayName
                };

                var result = await userManager.CreateAsync(user, RegisterViewModel.Password);

                if (result.Succeeded)
                {
                    return createUserObject(user);
                }

                return BadRequest("Problem Registering User");
            }

            return View();
        }
        [HttpGet("register")]
        public ViewResult Register()
        {
            var model = new RegisterViewModel();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            return createUserObject(user);
        }

        private UserDTO createUserObject(AppUser user)
        {
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
    }
}
