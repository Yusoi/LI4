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
    public class UserHandling
    {
        private readonly UserContext _context;
        public UserHandling(UserContext context)
        {
            _context = context;
        }

        public User[] getUsers()
        {
            return _context.user.ToArray();
        }

        public List<Ingredient> getExcludedIngredients(int userID)
        {
            var user = _context.user.Find(userID);
            if (user == null)
            {
                return null;
            }
            var excludedingredients = _context.excludedIngredients.Where(s => s.userID == userID);
            if (excludedingredients == null) return null;
            List<Ingredient> ingredients = new List<Ingredient>();
            foreach (ExcludedIngredients t in excludedingredients)
            {
                ingredients.Add(_context.ingredient.Find(t.ingredientID));
            }

            return ingredients;
        }

        public bool validateUser(User user)
        {
            user.password = MyHelpers.HashPassword(user.password);
            var returnedUser = _context.user.Where(b => b.email == user.email && b.password == user.password).FirstOrDefault();

            if (returnedUser == null)
            {
                return false;
            }
            return true;
        }

        public bool registerUser(User user)
        {
            user.password = MyHelpers.HashPassword(user.password);
            _context.user.Add(user);
            _context.SaveChanges();
            return true;
        }

    }
}