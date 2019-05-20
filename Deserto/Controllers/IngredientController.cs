using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;

namespace Deserto.Controllers
{ 
    [Route("api/[controller]")]
public class IngredientController : Controller
{

    private readonly UserContext _context;

    public IngredientController(UserContext context)
    {
        _context = context;
    }

    [HttpGet]
    public Ingredient[] Get()
    {
        return _context.ingredient.ToArray();
    }


    [HttpGet("{ingredientID}")]
    public ActionResult Get(int ingredientID)
    {
        var ingredient = _context.ingredient.Find(ingredientID);

        if (ingredient == null)
        {
            return NotFound();
        }

        return Ok(ingredient);
    }

    [HttpPost]
    public IActionResult Add([FromBody] Ingredient ingredient)
    {
        _context.ingredient.Add(ingredient);
        _context.SaveChanges();
        return new CreatedResult($"/api/ingredient/{ingredient.ingredientID}", ingredient);
    }

    [HttpDelete]
    public IActionResult Delete([FromQuery] int ingredientID)
    {
        var ingredient = _context.ingredient.Find(ingredientID);

        if (ingredient == null)
        {
            return NotFound();
        }
        _context.ingredient.Remove(ingredient);
        _context.SaveChanges();
        return NoContent();
    }
    }
}