using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deserto.Controllers
{
    [Route("api/[controller]")]
    public class ExcludedIngredientsController : Controller
    {

        private readonly UserContext _context;

        public ExcludedIngredientsController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ExcludedIngredients[] Get()
        {
            return _context.ExcludedIngredients.ToArray();
        }


        [HttpGet("{userID}/{ingredientID}")]
        public ActionResult Get(int userID, int ingredientID)
        {
            var excludedIngredients = _context.ExcludedIngredients.Find(userID, ingredientID);

            if (excludedIngredients == null)
            {
                return NotFound();
            }

            return Ok(excludedIngredients);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ExcludedIngredients excludedIngredients)
        {
            _context.ExcludedIngredients.Add(excludedIngredients);
            _context.SaveChanges();
            return new CreatedResult($"/api/recipe/{excludedIngredients.userID}/{excludedIngredients.ingredientID}", excludedIngredients);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int userID, int ingredientID)
        {
            var excludedIngredients = _context.ExcludedIngredients.Find(userID, ingredientID);

            if (excludedIngredients == null)
            {
                return NotFound();
            }
            _context.ExcludedIngredients.Remove(excludedIngredients);
            _context.SaveChanges();
            return NoContent();
        }


    }
}