﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deserto.Controllers
{
    [Route("api/[controller]")]
    public class ExplanationController : Controller
    {

        private readonly UserContext _context;

        public ExplanationController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Explanation[] Get()
        {
            return _context.explanation.ToArray();
        }


        [HttpGet("{explanationID}")]
        public ActionResult Get(int explanationID)
        {
            var explanation = _context.explanation.Find(explanationID);

            if (explanation == null)
            {
                return NotFound();
            }

            return Ok(explanation);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Explanation explanation)
        {
            _context.explanation.Add(explanation);
            _context.SaveChanges();
            return new CreatedResult($"/api/ingredient/{explanation.explanationID}", explanation);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int explanationID)
        {
            var explanation = _context.explanation.Find(explanationID);

            if (explanation == null)
            {
                return NotFound();
            }
            _context.explanation.Remove(explanation);
            _context.SaveChanges();
            return NoContent();
        }
    }
}