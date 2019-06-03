using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Deserto.Models;


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
            Debug.WriteLine("HelloHello");
            //Encontra todos os recipes originais
            List<Recipe> recipes = _context.Recipe.Where(r => r.original == -1).ToList();
            Debug.WriteLine("RecipesOriginais");
            foreach(Recipe r in recipes)
            {
                Debug.WriteLine(r.recipeID);
            }
            //Encontra todos os recipes que pertencem ao livro de receitas do utilizador
            List<RecipeBook> userRecipe = _context.RecipeBook.Where(u => u.userID == userID).ToList();
            Debug.WriteLine("UserRecipes");
            foreach(RecipeBook r in userRecipe)
            {
                Debug.WriteLine(r.recipeID + " " + r.userID);
            }
            //Placeholder para guardar a lista nova
            List<Recipe> tempRecipes = new List<Recipe>();


            foreach (Recipe r in recipes)
            {
                bool added = false;
                foreach (RecipeBook rb in userRecipe)
                { 
                    if(r.recipeID == getRecipe(rb.recipeID).original)
                    {
                        tempRecipes.Add(getRecipe(rb.recipeID));
                        added = true;
                    }
                }
                if (!added)
                {
                    tempRecipes.Add(r);
                } else
                {
                    added = false;
                }
            }

            Debug.WriteLine("TempRecipes");
            foreach (Recipe r in tempRecipes)
            {
                Debug.WriteLine(r.recipeID);
            }

            return tempRecipes;
        }

        public List<Recipe> getRecipes(string search, int userID)
        {
            //Encontra todos os recipes originais
            List<Recipe> recipes = _context.Recipe.Where(r => r.original == -1).ToList();
            //Encontra todos os recipes que pertencem ao livro de receitas do utilizador
            List<RecipeBook> userRecipe = _context.RecipeBook.Where(u => u.userID == userID).ToList();
            //Placeholder para guardar a lista nova
            List<Recipe> tempRecipes = new List<Recipe>();
            //Placeholder para guardar a lista após o search term
            List<Recipe> tempRecipes2 = new List<Recipe>();

            foreach (RecipeBook rb in userRecipe)
            {
                foreach (Recipe r in recipes)
                {
                    if (r.recipeID == getRecipe(rb.recipeID).original)
                    {
                        tempRecipes.Add(getRecipe(rb.recipeID));
                    }
                    else
                    {
                        tempRecipes.Add(r);
                    }
                }
            }

            if (!search.Equals(""))
            {
                foreach (Recipe r in tempRecipes)
                {
                    if (r.title.Contains(search))
                    {
                        tempRecipes2.Add(r);
                    }
                }
            }

            return tempRecipes2;
        }

        public Boolean belongsToRecipeBook(int recipeID, int userID)
        {
            if(_context.RecipeBook.Where(r => r.recipeID == recipeID && r.userID == userID).Count() != 0){
                return true;
            } else
            {
                return false;
            }
            
        }

        public List<Recipe> getUserRecipes(int userID)
        {
            Debug.WriteLine("HelloHello");
            var user = _context.User.Find(userID);
            if (user == null)
            {
                Debug.WriteLine("UserNotNull");
                return null;
            }
            var userrecipes = _context.RecipeBook.Where(s => s.userID == userID);
            Debug.WriteLine("UserRecipes:");
            foreach (RecipeBook r in userrecipes)
            {
                Debug.WriteLine(r.recipeID +" "+r.userID);
            }
            if (userrecipes == null)
            {
                Debug.WriteLine("UserRecipesNull");
                return null;
            }
                
            List<Recipe> recipes = new List<Recipe>();
            Debug.WriteLine("Recipes");
            foreach (RecipeBook t in userrecipes)
            {
                Debug.WriteLine(t.recipeID + " " + t.userID);
                recipes.Add(getRecipe(t.recipeID));
            }
            return recipes;
        }

        public Recipe getRecipe(int recipeID)
        {
            var recipe = _context.Recipe.Find(recipeID);
            if (recipe == null)
            {
                return null;
            }
            List<Ingredient> ings = getIngredients(recipeID);
            List<Instruction> insts = getInstructions(recipeID);
            recipe.ingredients = ings;
            recipe.instrucoes = insts;

            return recipe;
        }

        public void addToRecipeBook (int recipeID, int userID)
        {
            var recipe = _context.Recipe.Find(recipeID);
            if (recipe == null) return;

            if(_context.RecipeBook.Find(recipeID,userID) == null)
            {
                _context.RecipeBook.Add(new RecipeBook(recipeID, userID));
                _context.SaveChanges();
            }
        }

        public void removeFromRecipeBook( int receitaID,int userID)
        {
            var recipe = _context.Recipe.Find(receitaID);

            if (recipe == null)
            {
                return;
            }

            var aux = _context.RecipeBook.Find(receitaID,userID);

            if(aux == null)
            {
                return;
            }

            _context.RecipeBook.Remove(aux);
            _context.SaveChanges();

            if (recipe.original != -1)
            {
                cleanNotOriginalRecipe(recipe.recipeID);
                _context.Recipe.Remove(recipe);
                _context.SaveChanges();
            }
        }

        

        public List<Instruction> getInstructions(int recipeID)
        {
            List<Instruction> list = new List<Instruction>();
            var inst = _context.RecipeInstruction.Where(s => s.recipeID == recipeID);
            foreach (RecipeInstruction t in inst)
            {

                Instruction i = (_context.Instruction.Find(t.instructionID));
                i.order = t.ordernr;
                list.Add(i);

            }

            foreach (Instruction i in list)
            {
                var insExp = _context.InstructionExplanation.Where(s => s.instructionID == i.instructionID);
                if(insExp!=null)
                foreach (InstructionExplanation ie in insExp)
                {
                    Explanation explanation = (_context.Explanation.Find(ie.explanationID));
                    i.explanations.Add(explanation);
                }
            }
            List<Instruction> SortedList = list.OrderBy(o => o.order).ToList();
            return SortedList;
        }

        public List<Ingredient> getIngredients(int recipeID)
        {
            List<Ingredient> list = new List<Ingredient>();
            var inst = _context.RecipeIngredient.Where(s => s.recipeID == recipeID);
            foreach (RecipeIngredient t in inst)
            {

                Ingredient i = (_context.Ingredient.Find(t.ingredientID));
                i.quant = t.quant;
                list.Add(i);

            }
            return list;
        }

        public void cleanNotOriginalRecipe(int recipeID)
        {
            List<Instruction> instlist = getInstructions(recipeID);
            foreach (Instruction inst in instlist)
            {
                RecipeInstruction ri = _context.RecipeInstruction.Find(recipeID, inst.instructionID);
                _context.RecipeInstruction.Remove(ri);
                _context.SaveChanges();//colocar fora do ciclo probably
                _context.Instruction.Remove(inst);
                _context.SaveChanges();//colocar fora do ciclo probably
            }
            List<Ingredient> inglist = getIngredients(recipeID);
            foreach (Ingredient ing in inglist)
            {
                RecipeIngredient ri = _context.RecipeIngredient.Find(recipeID, ing.ingredientID);
                _context.RecipeIngredient.Remove(ri);
                _context.SaveChanges();//colocar fora do ciclo
                _context.Ingredient.Remove(ing);
                _context.SaveChanges();//colocar fora do ciclo
            }
        }

        public void addNotOriginalRecipe(Recipe oldrecipe,int id)
        {
            Recipe newrecipe = new Recipe(oldrecipe);

            if (oldrecipe.original == -1)
                newrecipe.original = oldrecipe.recipeID;
            else
                newrecipe.original = oldrecipe.original;

            _context.Recipe.Add(newrecipe);
            _context.SaveChanges();

            _context.RecipeBook.Add(new RecipeBook(newrecipe.recipeID, id));
            _context.SaveChanges();
            int i = 0;
            foreach (Instruction inst in oldrecipe.instrucoes)
            {
                Instruction newInst = new Instruction(inst);

                _context.Instruction.Add(newInst);
                _context.SaveChanges();

                RecipeInstruction ri = new RecipeInstruction(newrecipe.recipeID, newInst.instructionID, i);
                i++;

                _context.RecipeInstruction.Add(ri);
                _context.SaveChanges();//colocar fora do ciclo probably
            }
            foreach (Ingredient ing in oldrecipe.ingredients)
            {
                Ingredient newIng = new Ingredient(ing);
                _context.Ingredient.Add(newIng);
                _context.SaveChanges();
                RecipeIngredient ring = new RecipeIngredient(newrecipe.recipeID, newIng.ingredientID, ing.quant);

                _context.RecipeIngredient.Add(ring);
                _context.SaveChanges();//colocar fora do ciclo
            }
        }

        public void resetRecipe(int userID, int recipeID)
        {
            var oldr = _context.Recipe.Find(recipeID);
            var recipebook = _context.RecipeBook.Find(oldr.recipeID, userID);
            _context.RecipeBook.Remove(recipebook);
            _context.SaveChanges();
            if (oldr.original != -1)
            {

                cleanNotOriginalRecipe(oldr.recipeID);
                _context.Recipe.Remove(oldr);
                _context.SaveChanges();
            }
             RecipeBook rb = new RecipeBook(oldr.original, userID);
            var rbori = _context.RecipeBook.Find(oldr.original, userID);
            if (rbori == null)
            {
                _context.RecipeBook.Add(rb);
                _context.SaveChanges();
            }
        }

        public bool alreadyHasRating(int recipeID,int userID)
        {
           var ur = _context.UserRecipe.Find(userID, recipeID);
            if (ur == null) return false;
               return true;
        }

        public void setRecipeRating(int UserID, int RecipeID,char rating)
        {
           var userRecipe = _context.UserRecipe.Find(UserID, RecipeID);
            if (userRecipe == null)
            {
                UserRecipe urecipe = new UserRecipe(UserID, RecipeID, rating);
                _context.UserRecipe.Add(urecipe);
                _context.SaveChanges();
            }
        }

        public void addUpdatedRecipe(Recipe oldr,int id)
        {
            /*  oldr.imagePath = "DD";
              oldr.nutValues = " LALA";
              oldr.duration = 10;
              oldr.original = '0';
              oldr.difficulty = 'f';*/
            var recipebook = _context.RecipeBook.Find(oldr.recipeID, id);
            _context.RecipeBook.Remove(recipebook);
            _context.SaveChanges();
            if (oldr.original != -1)
            {

                cleanNotOriginalRecipe(oldr.recipeID);
                _context.Recipe.Remove(oldr);
                _context.SaveChanges();
            }
            
            addNotOriginalRecipe(oldr,id);
        }

        public List<RecipeInstruction> gettest(int recipeID)
        {
            List<RecipeInstruction> list = new List<RecipeInstruction>();
            var inst = _context.RecipeInstruction.Where(s => s.recipeID == recipeID);
            foreach (RecipeInstruction t in inst)
            {
                list.Add(t);
            }
            return list;
        }

        public float getMediaRating(int recipeID)
        {
            List<UserRecipe> ratings = _context.UserRecipe.Where(r => r.recipeID == recipeID).ToList();
            float sum = 0;
            int n = ratings.Count();
            foreach(UserRecipe r in ratings)
            {
                sum += float.Parse(r.rating.ToString());
            }
            float media = sum / (float)n;
            return (float)System.Math.Round(media,1);
        }

    }
}