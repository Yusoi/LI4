using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deserto.Controllers
{
    [Route("api/[controller]")]
    public class UserRecipeController : Controller
    {

        private readonly UserContext _context;

        public UserRecipeController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public UserRecipe[] Get()
        {
            return _context.UserRecipe.ToArray();
        }


        [HttpGet("{userID}/{recipeID}")]
        public ActionResult Get(int userID,int recipeID)
        {
            var recipe = _context.UserRecipe.Find(userID, recipeID);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserRecipe urecipe)
        {
            _context.UserRecipe.Add(urecipe);
            _context.SaveChanges();
            return new CreatedResult($"/api/recipe/{urecipe.recipeID}/{urecipe.userID}", urecipe);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int receitaID, int userID)
        {
            var recipe = _context.UserRecipe.Find(userID, receitaID);

            if (recipe == null)
            {
                return NotFound();
            }
            _context.UserRecipe.Remove(recipe);
            _context.SaveChanges();
            return NoContent();
        }
    }
}