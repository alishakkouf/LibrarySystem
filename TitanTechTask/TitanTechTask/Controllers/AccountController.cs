using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TitanTechTask.Domain.Account;
using TitanTechTask.Models;

namespace TitanTechTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<UserDomain> _signInManager;
        private readonly IUserManager _userManager; 

        public AccountController(SignInManager<UserDomain> signInManager, IUserManager userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);

            if (user != null)
            {
                // To compare the hashed password stored in the DB
                var passwordHasher = new PasswordHasher<UserDomain>();

                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    // sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }


        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Check if the user already exists in the database
            var existingUser = await _userManager.FindByEmailAsync(model.Username); 
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View();
            }

            // Create a new UserDomain object
            var newUser = new UserDomain
            {
                UserName = model.Username,
                NormalizedUserName = model.Username.ToUpper()
            };
            
            var passwordHasher = new PasswordHasher<UserDomain>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, model.Password);

            var createResult = await _userManager.CreateUserAsync(newUser); 

            if (createResult.Succeeded) 
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);

                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "An error occurred while creating the user.");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }


}
