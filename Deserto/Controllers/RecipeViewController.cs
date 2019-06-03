using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Speech;
using Models.shared;
using System.Speech.Synthesis;

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
                    r.MediaRating = recipeHandling.getMediaRating(r.recipeID);
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

        //Remover apartir da Pesquisa de Receitas
        public IActionResult removeFromRecipeBookRecipeSearch(int recipeID)
        {
            removeFromRecipeBook(recipeID);
            return RedirectToAction("getRecipes");
        }

        //Remover apartir do Livro de Receitas
        public IActionResult removeFromRecipeBookRecipeBook(int recipeID)
        {
            removeFromRecipeBook(recipeID);
            return RedirectToAction("getUserRecipes");
        }

        public void removeFromRecipeBook(int recipeID)
        {
            //TODO atualizar a página corretamente
            var identity = (ClaimsIdentity)User.Identity;
            int userID = Int32.Parse(identity.Name);

            recipeHandling.removeFromRecipeBook(recipeID, userID);
        }

        public IActionResult viewRecipe(int recipeID)
        {
            Recipe recipe = recipeHandling.getRecipe(recipeID);
            recipe.MediaRating = recipeHandling.getMediaRating(recipe.recipeID);
            return View(recipe);
        }

        public IActionResult getUserRecipes()
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);

            List<Recipe> recipes = recipeHandling.getUserRecipes(userid);
            foreach(Recipe r in recipes)
            {
                r.MediaRating = recipeHandling.getMediaRating(r.recipeID);
            }
            return View(recipes);
        }

        [Authorize]
        [HttpGet]
        public IActionResult displayInstruction(int recipeID)
        {
            TempData["ordernr"] = 0;
            TempData.Keep("ordernr");
            List<Instruction> list = recipeHandling.getInstructions(recipeID);
            TempData["RecipeID"] = recipeID;
            TempData["button"] = "False";
            TempData.Keep("RecipeID");
            TempData.Keep("button");
            return View(list.ElementAt(0));
        }

        //TODO: dar display da instrução correta
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
            int recipeID = (int) TempData["RecipeID"];
            TempData.Keep("RecipeID");
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);
            if (recipeHandling.alreadyHasRating(recipeID, userid))
                return RedirectToAction("UserPage", "UserView");
            return View(recipeHandling.getRecipe(recipeID));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Rating(Recipe recipe)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);
            Console.WriteLine("CARAHHDFHSDHHSDHFHHFHFFaaaaaaaaaa RATING" + "    " + recipe.Rating+ "  RECIPE ID " +recipe.recipeID);
            recipeHandling.setRecipeRating(userid, recipe.recipeID, recipe.Rating);
            return RedirectToAction("UserPage", "UserView");
        }

        [Authorize]
        [HttpGet]
        public IActionResult resetRecipe(int recipeID)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);
            recipeHandling.resetRecipe(userid, recipeID);
            return RedirectToAction("getUserRecipes", "RecipeView");
    }
        

        [Authorize]
        [HttpPost]
        public IActionResult End(int rating)
        {

            return View();
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> editarRecipe(int recipeID)
        {
            Recipe recipe = recipeHandling.getRecipe(recipeID);
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
            return RedirectToAction("getUserRecipes", "RecipeView");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var identity = (ClaimsIdentity)User.Identity;
            int userid = Int32.Parse(identity.Name);
            recipeHandling.removeFromRecipeBook(id, userid);
            return RedirectToAction("getUserRecipes", "RecipeView");
        }

    }
}