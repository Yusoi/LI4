using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult getExcludedIngredients()
        {
            List<Ingredient> ing = userHandling.getExcludedIngredients(userHandling.getCurrentUser());
            return View(ing);
        }

        public IActionResult getAllButExcludedIngredients()
        {
            List<Ingredient> ing = userHandling.getAllButExcludedIngredients(userHandling.getCurrentUser());
            return View(ing);
        }

        [HttpPost]
        public IActionResult addToExcludedIngredients([FromQuery] int ingredientID)
        {
            userHandling.addToExcludedIngredients(ingredientID);
            List<Ingredient> ing = userHandling.getAllButExcludedIngredients(userHandling.getCurrentUser());
            return View(ing);
        }

        [HttpDelete]
        public IActionResult removeFromExcludedIngredients([FromQuery] int ingredientID)
        {
            userHandling.removeFromExcludedIngredients(ingredientID);
            List<Ingredient> ing = userHandling.getExcludedIngredients(userHandling.getCurrentUser());
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
                    User logUser = userHandling.getUser(user.Email);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, logUser.UserID.ToString())
                    };
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    Thread.CurrentPrincipal = principal;

                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("UserPage", "UserView");

                }
                else
                {
                    TempData["UserLoginFailed"] = "Login Failed.Please enter correct credentials";
                }
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult UserPage()
        {

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