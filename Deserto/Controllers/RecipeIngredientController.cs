using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Deserto.Models;

namespace Deserto.Controllers
{
    [Route("api/[controller]")]
    public class RecipeIngredientController : Controller
    {

        private readonly UserContext _context;

        public RecipeIngredientController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public RecipeIngredient[] Get()
        {
            return _context.recipeIngredient.ToArray();
        }


        [HttpGet("{recipeID}/{ingredientID}")]
        public ActionResult Get(int recipeID, int ingredientID)
        {
            var recipeIngredient = _context.recipeIngredient.Find(recipeID, ingredientID);

            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return Ok(recipeIngredient);
        }

        [HttpPost]
        public IActionResult Add([FromBody] RecipeIngredient recipeIngredient)
        {
            _context.recipeIngredient.Add(recipeIngredient);
            _context.SaveChanges();
            return new CreatedResult($"/api/recipeIngredient/{recipeIngredient.recipeID}/{recipeIngredient.ingredientID}", recipeIngredient);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int reciepID, int ingredientID)
        {
            var recipeIngredient = _context.recipeIngredient.Find(reciepID, ingredientID);

            if (recipeIngredient == null)
            {
                return NotFound();
            }
            _context.recipeIngredient.Remove(recipeIngredient);
            _context.SaveChanges();
            return NoContent();
        }


    }
}