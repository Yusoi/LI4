using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.shared;


namespace Deserto.Controllers
{
    [Route("[controller]/[action]")]
    public class UserViewController : Controller
    {
        private UserHandling userHandling;
        public UserViewController(UserContext context)
        {
            //_context = context;
            userHandling = new UserHandling(context);
        }

        public IActionResult getUsers()
        {
            User[] users = userHandling.getUsers();
            return View(users);
        }

        public IActionResult getExludedIngredients()
        {
            List<Ingredient> ing = userHandling.getExcludedIngredients(1);
            return View(ing);
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser([Bind] User user)
        {
            if (ModelState.IsValid)
            {
                bool RegistrationStatus = this.userHandling.registerUser(user);
                if (RegistrationStatus)
                {
                    ModelState.Clear();
                    TempData["Success"] = "Registration Successful!";
                }
                else
                {
                    TempData["Fail"] = "This User ID already exists. Registration Failed.";
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult UserLogin()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogin([Bind] User user)
        {
            ModelState.Remove("nome");
            ModelState.Remove("email");

            if (ModelState.IsValid)
            {
                var LoginStatus = this.userHandling.validateUser(user);
                if (LoginStatus)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email)
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("getUsers", "UserView");
                }
                else
                {
                    TempData["UserLoginFailed"] = "Login Failed.Please enter correct credentials";
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("index", "Home");
        }
    }
}