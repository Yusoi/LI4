using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deserto.Controllers
{
    [Route("api/[controller]")]
    public class InstructionController : Controller
    {

        private readonly UserContext _context;

        public InstructionController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Instruction[] Get()
        {
            return _context.Instruction.ToArray();
        }


        [HttpGet("{instructionID}")]
        public ActionResult Get(int instructionID)
        {
            var instruction = _context.Instruction.Find(instructionID);

            if (instruction == null)
            {
                return NotFound();
            }

            return Ok(instruction);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Instruction instruction)
        {
            _context.Instruction.Add(instruction);
            _context.SaveChanges();
            return new CreatedResult($"/api/instruction/{instruction.instructionID}", instruction);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int instructionID)
        {
            var instruction = _context.Instruction.Find(instructionID);

            if (instruction == null)
            {
                return NotFound();
            }
            _context.Instruction.Remove(instruction);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("getExplanations/{instructionID}")]
        public ActionResult getRecipeInstructions(int instructionID)
        {
            var instruction = _context.Instruction.Find(instructionID);
            if (instruction == null)
            {
                return NotFound();
            }
            var explanations = _context.InstructionExplanation.Where(s => s.instructionID == instructionID);
            if (explanations == null) return NotFound();
            foreach (InstructionExplanation t in explanations)
            {
                instruction.InstructionExplanations.Add(t);
            }

            return Ok(instruction);
        }
    }
}