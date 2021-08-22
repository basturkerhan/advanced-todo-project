using AdvancedTodoApplication.Models;
using AdvancedTodoApplication.Repository;
using AdvancedTodoApplication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;

        public AccountController( IAccountRepository accountRepository, 
                                  IUserService userService )
        {
            _accountRepository = accountRepository;
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Details()
        {
            string userId = _userService.GetUserId();
            ApplicationUser user = await _userService.GetUserById(userId);

            return View(user);
        }


        // SIGNUP -> GET AND POST
        [Route("signup")]
        public IActionResult Signup()
        {
            if (_userService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Board");
            }
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(SignUpUserModel userModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                if(!result.Succeeded)
                {
                    foreach(var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }

                ModelState.Clear();
            }
            return View();
        }


        // SIGNIN -> GET AND POST
        [Route("login")]
        public IActionResult Login()
        {
            if (_userService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Board");
            }
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(signInModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Board");
                }

                ModelState.AddModelError("", "Geçersiz veriler girildi");
            }
            return View(signInModel);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }
}
