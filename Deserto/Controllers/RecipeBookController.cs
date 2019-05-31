using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Deserto.Controllers
{

    [Route("api/[controller]")]
    public class RecipeBookController : Controller
    {
        private readonly UserContext _context;

        public RecipeBookController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public RecipeBook[] Get()
        {
            return _context.RecipeBook.ToArray();
        }


        [HttpGet("{recipeID}/{userID}")]
        public ActionResult Get(int recipeID, int userID)
        {
            var recipeBook = _context.RecipeBook.Find(recipeID, userID);

            if (recipeBook == null)
            {
                return NotFound();
            }

            return Ok(recipeBook);
        }

        [HttpPost]
        public IActionResult Add([FromBody] RecipeBook recipeBook)
        {
            _context.RecipeBook.Add(recipeBook);
            _context.SaveChanges();
            return new CreatedResult($"/api/recipeBook/{recipeBook.recipeID}/{recipeBook.userID}", recipeBook);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int recipeID, int userID)
        {
            var recipeBook = _context.RecipeBook.Find(recipeID, userID);

            if (recipeBook == null)
            {
                return NotFound();
            }
            _context.RecipeBook.Remove(recipeBook);
            _context.SaveChanges();
            return NoContent();
        }
    }
}