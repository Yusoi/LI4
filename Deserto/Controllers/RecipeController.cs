using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deserto.Models;

namespace Deserto.Controllers
{
    [Route("api/[controller]")]
    public class RecipeController : Controller
    {

        private readonly UserContext _context;

        public RecipeController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Recipe[] Get()
        {
            return _context.recipe.ToArray();
        }


        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var recipe = _context.recipe.Find(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Recipe recipe)
        {
            _context.recipe.Add(recipe);
            _context.SaveChanges();
            return new CreatedResult($"/api/recipe/{recipe.recipeID}", recipe);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int receitaID)
        {
            var recipe = _context.recipe.Find(receitaID);

            if (recipe == null)
            {
                return NotFound();
            }
            _context.recipe.Remove(recipe);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpGet("getingredients/{recipeID}")]
        public ActionResult getRecipeIngredients(int recipeID)
        {
            var recipe = _context.recipe.Find(recipeID);
            if (recipe == null)
            {
                return NotFound();
            }
            var ingredients = _context.recipeIngredient.Where(s => s.recipeID == recipeID);
            if (ingredients == null) return NotFound();
            foreach (RecipeIngredient t in ingredients)
            {
                recipe.RecipeIngredient.Add(t);
            }

            return Ok(recipe);
        }

        [HttpGet("getInstructions/{recipeID}")]
        public ActionResult getRecipeInstructions(int recipeID)
        {
            var recipe = _context.recipe.Find(recipeID);
            if (recipe == null)
            {
                return NotFound();
            }
            var instructions = _context.recipeInstruction.Where(s => s.recipeID == recipeID);
            if (instructions == null) return NotFound();
            foreach (RecipeInstruction t in instructions)
            {
                recipe.RecipeInstruction.Add(t);
            }

            return Ok(recipe);
        }
    }
}