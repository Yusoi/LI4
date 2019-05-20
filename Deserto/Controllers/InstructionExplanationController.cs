using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deserto.Controllers
{
    [Route("api/[controller]")]
    public class InstructionExplanationController : Controller
    {

        private readonly UserContext _context;

        public InstructionExplanationController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public InstructionExplanation[] Get()
        {
            return _context.instructionExplanation.ToArray();
        }


        [HttpGet("{instructionID}/{explanationID}")]
        public ActionResult Get(int instructionID, int explanationID)
        {
            var instructionExplanation = _context.instructionExplanation.Find(instructionID, explanationID);

            if (instructionExplanation == null)
            {
                return NotFound();
            }

            return Ok(instructionExplanation);
        }

        [HttpPost]
        public IActionResult Add([FromBody] InstructionExplanation instructionExplanation)
        {
            _context.instructionExplanation.Add(instructionExplanation);
            _context.SaveChanges();
            return new CreatedResult($"/api/recipeIngredient/{instructionExplanation.instructionID}/{instructionExplanation.explanationID}", instructionExplanation);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int instructionID, int explanationID)
        {
            var instructionExplanation = _context.instructionExplanation.Find(instructionID, explanationID);

            if (instructionExplanation == null)
            {
                return NotFound();
            }
            _context.instructionExplanation.Remove(instructionExplanation);
            _context.SaveChanges();
            return NoContent();
        }


    }
}