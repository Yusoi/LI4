
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deserto.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Deserto
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public User[] Get()
        {
            return _context.user.ToArray();
        }


        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var user = _context.user.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Add([FromBody] User user)
        {
            _context.user.Add(user);
            _context.SaveChanges();
            return new CreatedResult($"/api/user/{user.userID}", user);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int userID)
        {
            var user = _context.user.Find(userID);

            if (user == null)
            {
                return NotFound();
            }
            _context.user.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("getrecipes/{userID}")]
        public ActionResult getUserRecipes(int userID)
        {
            var user = _context.user.Find(userID);
            if (user == null)
            {
                return NotFound();
            }
            var recipes = _context.userRecipe.Where(s => s.userID == userID);
            if (recipes == null) return NotFound();
            foreach (UserRecipe t in recipes)
            {
                user.UserRecipes.Add(t);
            }

            return Ok(user);
        }

        [HttpGet("getexcludedIngredients/{userID}")]
        public ActionResult getuserExludedIngredients(int userID)
        {
            var user = _context.user.Find(userID);
            if (user == null)
            {
                return NotFound();
            }
            var ingredients = _context.excludedIngredients.Where(s => s.userID == userID);
            if (ingredients == null) return NotFound();
            foreach (ExcludedIngredients t in ingredients)
            {
                user.ExcludedIngredients.Add(t);
            }

            return Ok(user);
        }
    }
}