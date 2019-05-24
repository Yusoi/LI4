using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deserto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Deserto.shared;

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