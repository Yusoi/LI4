using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Authorization;
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
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);

            List<Recipe> recipes = recipeHandling.getRecipes(userid);
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
            TempData["ordernr"] = 0;
            List<Instruction> list = recipeHandling.getInstructions(4);
            return View(list.ElementAt(0));
        }

        [HttpPost]
        public IActionResult displayInstruction(Instruction r)
        {
            int ind = (int)TempData["ordernr"];
            TempData["ordernr"] = ind + 1;
            TempData.Keep("ordernr");
            List<Instruction> list = recipeHandling.getInstructions(4);

            if (list.Count() != ind + 2)
                return View(list.ElementAt((int)TempData["ordernr"]));

            TempData["button"] = "false";
            return View(list.ElementAt((int)TempData["ordernr"]));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> editarRecipe(int id)
        {
            Recipe recipe = recipeHandling.getRecipe(id);
            //Console.Write("INGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG  {0}",recipe.ingredients.Count());
            return View(recipe);
        }
        [Authorize]
        [HttpPost]
        public IActionResult editarRecipe(Recipe recipe)
        {
            var identity = (ClaimsIdentity)User.Identity;
            string c = identity.Name;
            Console.WriteLine("AAAAAAAAAAAAAAAAAsize: " + recipe.ingredients.Count + " ");
            foreach(Ingredient asd in recipe.ingredients){
                Console.WriteLine("CARALHOOOOOOOOOOOOOOO   " + asd.name + "   QUANT  " + asd.quant);
            }
            recipeHandling.addUpdatedRecipe(recipe, Int32.Parse(c));
            return RedirectToAction("getRecipes", "RecipeView");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);
            recipeHandling.removeRecipeDoLivro(id, userid);
            return RedirectToAction("getRecipes", "RecipeView");
        }

    }
}