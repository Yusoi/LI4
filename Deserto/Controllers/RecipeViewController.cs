using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;
using Models.shared;

namespace Deserto.Controllers
{
    [Route("[controller]/[action]")]
    public class RecipeViewController : Controller
    {
        private RecipeHandling recipeHandling;
        public RecipeViewController(UserContext context)
        {
            //_context = context;
            recipeHandling = new RecipeHandling(context);
        }

        public IActionResult getRecipes()
        {
            List<Recipe> recipes = recipeHandling.getRecipes(1);
            return View(recipes);
        }
        public IActionResult getRecipe()
        {
            Recipe recipe = recipeHandling.getRecipe(4);
            return View(recipe);
        }
        [HttpGet]
        public IActionResult displayInstruction(int i)
        {
            TempData["teste"] = 0;
            List<Instruction> list = recipeHandling.getInstructions(4);
            return View(list.ElementAt(0));
        }

        [HttpPost]
        public IActionResult displayInstruction(Instruction r)
        {
            int ind = (int)TempData["teste"];
            TempData["teste"] = ind + 1;
            TempData.Keep("teste");
            List<Instruction> list = recipeHandling.getInstructions(4);

            if (list.Count() != ind + 2)
                return View(list.ElementAt((int)TempData["teste"]));

            TempData["button"] = "false";
            return View(list.ElementAt((int)TempData["teste"]));
        }

    }
}