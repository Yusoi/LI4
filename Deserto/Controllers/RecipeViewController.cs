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

        [HttpGet("{search?}")]
        public IActionResult getRecipes(string search)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userID = Int32.Parse(identity.Name);

            List<Recipe> recipes;
            if (search == null)
            {
                recipes = recipeHandling.getRecipes(userID);
            } else
            {
                recipes = recipeHandling.getRecipes(search,userID);
            }

            //Verifica se pertence ao livro de receitas para mostrar o "remove from recipe book"
            //TODO matching com cópias da receita
            foreach (Recipe r in recipes)
            {
                if (recipeHandling.belongsToRecipeBook(r.recipeID, userID))
                {
                    TempData[r.recipeID.ToString()] = 0;
                    TempData.Keep();
                }
            }

            return View(recipes);
        }

        public IActionResult addToRecipeBook(int recipeID)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userID = Int32.Parse(identity.Name);

            recipeHandling.addToRecipeBook(recipeID, userID);
            return RedirectToAction("getRecipes");
        }

        public IActionResult removeFromRecipeBook(int recipeID)
        {
            //TODO atualizar a página corretamente
            var identity = (ClaimsIdentity)User.Identity;
            int userID = Int32.Parse(identity.Name);

            recipeHandling.removeFromRecipeBook(recipeID, userID);
            return RedirectToAction("getRecipes");
        }

        public IActionResult getUserRecipes()
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);

            List<Recipe> recipes = recipeHandling.getUserRecipes(userid);
            return View(recipes);
        }
        public IActionResult getRecipe()
        {
            Recipe recipe = recipeHandling.getRecipe(4);
            return View(recipe);
        }
        [Authorize]
        [HttpGet]
        public IActionResult displayInstruction(int i)
        {
            TempData["ordernr"] = 0;
            TempData.Keep("ordernr");
            List<Instruction> list = recipeHandling.getInstructions(4);
            TempData["RecipeID"] = 4;
            TempData["button"] = "False";
            TempData.Keep("RecipeID");
            TempData.Keep("button");
            return View(list.ElementAt(0));
        }
        [Authorize]
        [HttpPost]
        public IActionResult displayInstruction(string ordem)
        {
            TempData["button"] = "False";
            TempData.Keep("button");
            int ind = (int)TempData["ordernr"];
            if (ordem.Equals("Next"))
                ind = ind + 1;
            if (ordem.Equals("Back"))
                ind = ind- 1;
            TempData["ordernr"] = ind;
            TempData.Keep("ordernr");
            
            List <Instruction> list = recipeHandling.getInstructions(4);
            Console.WriteLine("CARAHHDFHSDHHSDHFHHFHFFaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" + TempData["ordernr"] + "IND + 2 = " + (ind + 2)+ "conoutt = "+list.Count());
            if (list.Count() != ind + 1)
                return View(list.ElementAt((int)TempData["ordernr"]));

            TempData["button"] = "True";
            TempData.Keep("button");
            return View(list.ElementAt((int)TempData["ordernr"]));
        }
        [Authorize]
        [HttpGet]
        public IActionResult Rating()
        {
           
            return View(recipeHandling.getRecipe(4));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Rating(Recipe recipe)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);
            Console.WriteLine("CARAHHDFHSDHHSDHFHHFHFFaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" + "    " + recipe.Rating);
            recipeHandling.setRecipeRating(userid, recipe.recipeID, recipe.Rating);
            return View();
        }



        [Authorize]
        [HttpPost]
        public IActionResult End(int rating)
        {

            return View();
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
            recipeHandling.removeFromRecipeBook(id, userid);
            return RedirectToAction("getRecipes", "RecipeView");
        }

    }
}