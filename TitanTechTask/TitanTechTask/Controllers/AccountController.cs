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
                // Use PasswordHasher to compare the hashed password stored in the DB
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

            // Use ASP.NET Core Identity PasswordHasher to hash the password
            var passwordHasher = new PasswordHasher<UserDomain>();
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, model.Password);

            // Save the user using your data access layer (UserProvider)
            var createResult = await _userManager.CreateUserAsync(newUser); // Custom method to insert the user into DB

            if (createResult.Succeeded) // Assuming the result is a boolean indicating success
            {
                // Manually sign in the user (assuming _signInManager is configured for manual sign-ins)
                await _signInManager.SignInAsync(newUser, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            // Handle errors if user creation failed
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
