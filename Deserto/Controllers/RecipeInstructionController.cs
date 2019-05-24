using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deserto.Controllers
{
    [Route("api/[controller]")]
    public class RecipeInstructionController : Controller
    {

        private readonly UserContext _context;

        public RecipeInstructionController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public RecipeInstruction[] Get()
        {
            return _context.RecipeInstruction.ToArray();
        }


        [HttpGet("{recipeID}/{instructionID}")]
        public ActionResult Get(int recipeID, int instructionID)
        {
            var recipeInstruction = _context.RecipeInstruction.Find(recipeID, instructionID);

            if (recipeInstruction == null)
            {
                return NotFound();
            }

            return Ok(recipeInstruction);
        }

        [HttpPost]
        public IActionResult Add([FromBody] RecipeInstruction recipeInstruction)
        {
            _context.RecipeInstruction.Add(recipeInstruction);
            _context.SaveChanges();
            return new CreatedResult($"/api/recipeIngredient/{recipeInstruction.recipeID}/{recipeInstruction.instructionID}", recipeInstruction);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int reciepID, int instructionID)
        {
            var recipeInstruction = _context.RecipeInstruction.Find(reciepID, instructionID);

            if (recipeInstruction == null)
            {
                return NotFound();
            }
            _context.RecipeInstruction.Remove(recipeInstruction);
            _context.SaveChanges();
            return NoContent();
        }

    }
}