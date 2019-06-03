using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Deserto.shared;
using System.Security.Claims;

namespace Models.shared
{
    public class UserHandling
    {
        private readonly UserContext _context;
        public UserHandling(UserContext context)
        {
            _context = context;
        }

        public User[] getUsers()
        {
            return _context.User.ToArray();
        }

        public List<Ingredient> getExcludedIngredients(int userID)
        {
            var user = _context.User.Find(userID);
            if (user == null)
            {
                return null;
            }
            var excludedingredients = _context.ExcludedIngredients.Where(s => s.userID == userID);
            if (excludedingredients == null) return null;
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (ExcludedIngredients t in excludedingredients)
            {
                ingredients.Add(_context.Ingredient.Find(t.ingredientID));
            }

            return ingredients;
        }

        public List<Ingredient> getAllButExcludedIngredients(int userID)
        {
            var user = _context.User.Find(userID);
            if (user == null)
            {
                return null;
            }
            List<ExcludedIngredients> excludedIngredients = _context.ExcludedIngredients.Where(s => s.userID == userID).ToList();
            List<Ingredient> ingredients = _context.Ingredient.ToList();
            List<Ingredient> nonExcludedIngredients = new List<Ingredient>();
            foreach(Ingredient i in ingredients)
            {
                var excluded = false;

                foreach(ExcludedIngredients ei in excludedIngredients)
                {
                    if(i.ingredientID == ei.ingredientID)
                    {
                        excluded = true;
                        break;
                    }
                }

                if(excluded == false)
                {
                    nonExcludedIngredients.Add(i);
                }
            }

            return nonExcludedIngredients;
        }

        [HttpPost]
        public void addToExcludedIngredients(int ingredientID, int userID)
        {
            ExcludedIngredients excludedIngredients = new ExcludedIngredients();
            excludedIngredients.ingredientID = ingredientID;
            excludedIngredients.userID = userID;
            _context.ExcludedIngredients.Add(excludedIngredients);
            _context.SaveChanges();
            //return new CreatedResult($"/api/recipe/{excludedIngredients.userID}/{excludedIngredients.ingredientID}", excludedIngredients);
        }

        [HttpDelete]
        public void removeFromExcludedIngredients(int ingredientID, int userID)
        {
            var excludedIngredients = _context.ExcludedIngredients.Find(userID, ingredientID);

            if (excludedIngredients == null)
            {
                return;
            }
            _context.ExcludedIngredients.Remove(excludedIngredients);
            _context.SaveChanges();
        }

        public User getUser(string Email)
        {
             var user = _context.User.Where(b => b.Email == Email).FirstOrDefault();
            if (user == null) return null;
            User returnedUser = (User) user;
            return returnedUser;
        }

        public User getUser(int userID)
        {
            var user = _context.User.Where(b => b.UserID == userID).FirstOrDefault();
            if (user == null) return null;
            User returnedUser = (User) user;
            return returnedUser;
        }

        public bool validateUser(User user)
        {
            user.Password = MyHelpers.HashPassword(user.Password);
            var returnedUser = _context.User.Where(b => b.Email == user.Email && b.Password == user.Password).FirstOrDefault();

            if (returnedUser == null)
            {
                return false;
            }
            return true;
        }

        public bool registerUser(User user)
        {
            user.Password = MyHelpers.HashPassword(user.Password);
            _context.User.Add(user);
            _context.SaveChanges();
            return true;
        }

    }
}