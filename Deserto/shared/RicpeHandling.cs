using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Deserto.Models;
using System.Collections;
using Deserto.shared;

namespace Models.shared
{
    public class RecipeHandling
    {
        private readonly UserContext _context;
        public RecipeHandling(UserContext context)
        {
            _context = context;
        }

        public List<Recipe> getRecipes(int userID)
        {
            var user = _context.user.Find(userID);
            if (user == null)
            {
                return null;
            }
            var userrecipes = _context.userRecipe.Where(s => s.userID == userID);
            if (userrecipes == null) return null;
            List<Recipe> recipes = new List<Recipe>();
            foreach (UserRecipe t in userrecipes)
            {
                recipes.Add(_context.recipe.Find(t.recipeID));
            }
            return recipes;
        }

        public Recipe getRecipe(int userID)
        {
            var recipe = _context.recipe.Find(userID);
            if (recipe == null)
            {
                return null;
            }
            return recipe;
        }

        public List<Instruction> getInstructions(int recipeID)
        {
            List<Instruction> list = new List<Instruction>();
            var inst = _context.recipeInstruction.Where(s => s.recipeID == recipeID);
            foreach (RecipeInstruction t in inst)
            {

                Instruction i = (_context.instruction.Find(t.instructionID));
                i.order = t.ordernr;
                list.Add(i);

            }
            List<Instruction> SortedList = list.OrderBy(o => o.order).ToList();
            return SortedList;
        }

        public List<RecipeInstruction> gettest(int recipeID)
        {
            List<RecipeInstruction> list = new List<RecipeInstruction>();
            var inst = _context.recipeInstruction.Where(s => s.recipeID == recipeID);
            foreach (RecipeInstruction t in inst)
            {
                list.Add(t);
            }
            return list;
        }

    }
}